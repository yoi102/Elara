using Microsoft.AspNetCore.SignalR.Client;

namespace EventBus.SignalR.Client;
public static class EventHandlerRegistrar
{
    public static void MapHandlers(HubConnection connection, IServiceProvider provider)
    {
        foreach (var item in ServicesCollectionExtensions.NonGenericHandlersByEvent)
        {
            connection.On(item.Key, async () =>
            {
                foreach (var handlerType in item.Value)
                {
                    var handler = provider.GetService(handlerType);

                    if (handler is not IEventHandler eventHandler)
                        continue;

                    await eventHandler.HandleAsync();
                }
            });
        }

        foreach (var item in ServicesCollectionExtensions.GenericHandlersByEvent)
        {
            connection.On(item.Key, [item.Value.EventType], async (objects) =>
            {
                foreach (var handlerType in item.Value.HandlerTypes)
                {
                    var eventDataType = item.Value.EventType;
                    var handler = provider.GetService(eventDataType);
                    if (handler == null) continue;

                    var interfaceType = typeof(IEventHandler<>).MakeGenericType(handlerType);
                    var handleMethod = interfaceType.GetMethod(nameof(IEventHandler.HandleAsync));

                    if (handleMethod != null)
                    {
                        var task = (Task?)handleMethod.Invoke(handler, [objects[0]]);
                        if (task != null) await task;
                    }
                }
            });
        }
    }
}
