namespace EventBus;

public interface IEventBus
{
    Task PublishAsync(string eventName, object? eventData);

    Task SubscribeAsync(string eventName, Type handlerType);

    Task Unsubscribe(string eventName, Type handlerType);
}