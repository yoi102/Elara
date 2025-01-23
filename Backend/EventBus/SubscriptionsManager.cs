using EventBus.Handlers;

namespace EventBus;

internal class SubscriptionsManager
{
    //key是eventName，值是监听这个事件的实现了IIntegrationEventHandler接口的类型
    private readonly Dictionary<string, List<Type>> handlers = new Dictionary<string, List<Type>>();

    public event AsyncEventHandler<string>? OnAllEventRemovedAsync;

    public bool IsEmpty => handlers.Count == 0;

    public void AddSubscription(string eventName, Type eventHandlerType)
    {
        if (!HasSubscriptionsForEvent(eventName))
        {
            handlers.Add(eventName, new List<Type>());
        }

        if (handlers[eventName].Contains(eventHandlerType))
        {
            throw new ArgumentException($"Handler Type {eventHandlerType} already registered for '{eventName}'", nameof(eventHandlerType));
        }
        handlers[eventName].Add(eventHandlerType);
    }

    public void Clear() => handlers.Clear();

    public IEnumerable<Type> GetHandlersForEvent(string eventName) => handlers[eventName];

    public bool HasSubscriptionsForEvent(string eventName) => handlers.ContainsKey(eventName);

    public async Task RemoveSubscription(string eventName, Type handlerType)
    {
        handlers[eventName].Remove(handlerType);
        if (handlers[eventName].Count == 0)
        {
            handlers.Remove(eventName);
            await Task.Run(() =>
              {
                  OnAllEventRemovedAsync?.Invoke(this, eventName);
              });
        }
    }
}