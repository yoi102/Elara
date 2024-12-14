using EventBus.Handlers;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace EventBus;

internal class RabbitMQEventBus : IEventBus, IDisposable
{
    private readonly IServiceProvider _serviceProvider;
    private readonly SubscriptionsManager consumerChannelSubscriptionsManager;
    private readonly string exchangeName;
    private readonly RabbitMQConnection persistentConnection;
    private readonly IServiceScope serviceScope;
    private IChannel _consumerChannel;
    private string queueName;

    public RabbitMQEventBus(RabbitMQConnection persistentConnection,
        IServiceScopeFactory serviceProviderFactory, string exchangeName, string queueName)
    {
        this.persistentConnection = persistentConnection ?? throw new ArgumentNullException(nameof(persistentConnection));
        consumerChannelSubscriptionsManager = new SubscriptionsManager();
        this.exchangeName = exchangeName;
        this.queueName = queueName;
        _consumerChannel = CreateConsumerChannelAsync().Result;

        //因为RabbitMQEventBus是Singleton对象，而它创建的IIntegrationEventHandler以及用到的IIntegrationEventHandler用到的服务
        //大部分是Scoped，因此必须这样显式创建一个 scope，否则在getservice的时候会报错：Cannot resolve from root provider because it requires scoped service
        serviceScope = serviceProviderFactory.CreateScope();
        _serviceProvider = serviceScope.ServiceProvider;
        consumerChannelSubscriptionsManager.OnAllEventRemovedAsync += SubscriptionsManager_OnAllEventRemoved;
    }

    public void Dispose()
    {
        _consumerChannel.Dispose();
        consumerChannelSubscriptionsManager.Clear();
        persistentConnection.Dispose();
        serviceScope.Dispose();
    }

    public async Task PublishAsync(string eventName, object? eventData)
    {
        if (!persistentConnection.IsConnected)
        {
            await persistentConnection.TryConnectAsync();
        }
        //Channel 是建立在 Connection 上的虚拟连接
        //创建和销毁 TCP 连接的代价非常高，
        //Connection 可以创建多个 Channel ，Channel 不是线程安全的所以不能在线程间共享。
        using var publishChannel = await persistentConnection.CreateChannelAsync();
        await publishChannel.ExchangeDeclareAsync(exchange: exchangeName, type: "direct");

        byte[] body;
        if (eventData == null)
        {
            body = [];
        }
        else
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            body = JsonSerializer.SerializeToUtf8Bytes(eventData, eventData.GetType(), options);
        }

        await publishChannel.BasicPublishAsync(
             exchange: exchangeName,
             routingKey: eventName,
             mandatory: true,
             basicProperties: new BasicProperties() { DeliveryMode = DeliveryModes.Persistent },
             body: body);
    }

    public async Task SubscribeAsync(string eventName, Type handlerType)
    {
        CheckHandlerType(handlerType);
        await DoInternalSubscription(eventName);
        consumerChannelSubscriptionsManager.AddSubscription(eventName, handlerType);
        await StartBasicConsumeAsync();
    }

    public async Task Unsubscribe(string eventName, Type handlerType)
    {
        CheckHandlerType(handlerType);
        await consumerChannelSubscriptionsManager.RemoveSubscription(eventName, handlerType);
    }

    private void CheckHandlerType(Type handlerType)
    {
        if (!typeof(IIntegrationEventHandler).IsAssignableFrom(handlerType))
        {
            throw new ArgumentException($"{handlerType} doesn't inherit from IIntegrationEventHandler", nameof(handlerType));
        }
    }

    private async Task ConsumerReceivedAsync(object sender, BasicDeliverEventArgs eventArgs)
    {
        var eventName = eventArgs.RoutingKey;//这个框架中，就是用 eventName当 RoutingKey
        var message = Encoding.UTF8.GetString(eventArgs.Body.Span);//框架要求所有的消息都是字符串的 json
        try
        {
            await ProcessEvent(eventName, message);
            //如果在获取消息时采用不自动应答，但是获取消息后不调用 BasicAckAsync，
            //RabbitMQ会认为消息没有投递成功，不仅所有的消息都会保留到内存中，
            //而且在客户重新连接后，会将消息重新投递一遍。这种情况无法完全避免，因此EventHandler的代码需要幂等
            await _consumerChannel.BasicAckAsync(eventArgs.DeliveryTag, multiple: false);
            //multiple：批量确认标志。如果值为 true，则执行批量确认，此 deliveryTag之前收到的消息全部进行确认; 如果值为 false，则只对当前收到的消息进行确认
        }
        catch (Exception ex)
        {
            //requeue：表示如何处理这条消息，如果值为 true，则重新放入RabbitMQ的发送队列，如果值为 false，则通知RabbitMQ销毁这条消息
            await _consumerChannel.BasicRejectAsync(eventArgs.DeliveryTag, true);
            Debug.Fail(ex.ToString());
        }
    }

    private async Task<IChannel> CreateConsumerChannelAsync()
    {
        if (!persistentConnection.IsConnected)
        {
            await persistentConnection.TryConnectAsync();
        }

        var consumerChannel = await persistentConnection.CreateChannelAsync();
        await consumerChannel.ExchangeDeclareAsync(exchange: exchangeName,
                                    type: "direct");

        await consumerChannel.QueueDeclareAsync(queue: queueName,
                             durable: true,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        consumerChannel.CallbackExceptionAsync += async (sender, ea) =>
       {
           await _consumerChannel.DisposeAsync();
           _consumerChannel = await CreateConsumerChannelAsync();
           await StartBasicConsumeAsync();
           Debug.Fail(ea.ToString());
       };
        return consumerChannel;
    }

    private async Task DoInternalSubscription(string eventName)
    {
        var containsKey = consumerChannelSubscriptionsManager.HasSubscriptionsForEvent(eventName);
        if (containsKey)
            return;

        if (!persistentConnection.IsConnected)
        {
            await persistentConnection.TryConnectAsync();
        }
        if (_consumerChannel?.IsOpen != true)
        {
            _consumerChannel = await CreateConsumerChannelAsync();
        }

        await _consumerChannel.QueueBindAsync(queue: queueName,
                              exchange: exchangeName,
                              routingKey: eventName);
    }

    private async Task ProcessEvent(string eventName, string message)
    {
        if (consumerChannelSubscriptionsManager.HasSubscriptionsForEvent(eventName))
        {
            var subscriptions = consumerChannelSubscriptionsManager.GetHandlersForEvent(eventName);
            foreach (var subscription in subscriptions)
            {
                //各自在不同的Scope中，避免DbContext等的共享造成如下问题：
                //The instance of entity type cannot be tracked because another instance
                using var scope = _serviceProvider.CreateScope();
                var handler = scope.ServiceProvider.GetService(subscription) as IIntegrationEventHandler;
                if (handler == null)
                    throw new ApplicationException($"无法创建{subscription}类型的服务");

                await handler.Handle(eventName, message);
            }
        }
        else
        {
            var assembly = Assembly.GetEntryAssembly();
            if (assembly == null)
                throw new ApplicationException("Failed to retrieve the entry assembly.");
            var entryAsm = assembly.GetName().Name;
            Debug.WriteLine($"找不到可以处理 eventName={eventName}的处理程序，entryAsm:{entryAsm}");
        }
    }

    private async Task StartBasicConsumeAsync()
    {
        if (_consumerChannel?.IsOpen != true)
            return;
        var consumer = new AsyncEventingBasicConsumer(_consumerChannel);
        consumer.ReceivedAsync += ConsumerReceivedAsync;
        await _consumerChannel.BasicConsumeAsync(
             queue: queueName,
             autoAck: false,
             consumer: consumer);
    }

    private async Task SubscriptionsManager_OnAllEventRemoved(object? sender, string eventName)
    {
        if (_consumerChannel?.IsOpen != true)
            return;

        if (!persistentConnection.IsConnected)
        {
            await persistentConnection.TryConnectAsync();
        }

        await _consumerChannel.QueueUnbindAsync(queue: queueName,
                  exchange: exchangeName,
                  routingKey: eventName);

        if (consumerChannelSubscriptionsManager.IsEmpty)
        {
            queueName = string.Empty;

            await _consumerChannel.CloseAsync();
        }
    }
}