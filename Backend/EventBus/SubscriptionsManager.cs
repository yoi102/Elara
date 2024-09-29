using System;
using System.Collections.Generic;
using System.Linq;

namespace EventBus
{
    internal class SubscriptionsManager
    {
        //key是eventName，值是监听这个事件的实现了IIntegrationEventHandler接口的类型
        private readonly Dictionary<string, List<Type>> _handlers = new Dictionary<string, List<Type>>();

        public event EventHandler<string>? OnEventRemoved;

        public bool IsEmpty => !_handlers.Keys.Any();


        public void AddSubscription(string eventName, Type eventHandlerType)
        {
            if (!HasSubscriptionsForEvent(eventName))
            {
                _handlers.Add(eventName, new List<Type>());
            }

            if (_handlers[eventName].Contains(eventHandlerType))
            {
                throw new ArgumentException($"Handler Type {eventHandlerType} already registered for '{eventName}'", nameof(eventHandlerType));
            }
            _handlers[eventName].Add(eventHandlerType);
        }

        public void Clear() => _handlers.Clear();


        public IEnumerable<Type> GetHandlersForEvent(string eventName) => _handlers[eventName];


        public bool HasSubscriptionsForEvent(string eventName) => _handlers.ContainsKey(eventName);

        public void RemoveSubscription(string eventName, Type handlerType)
        {
            _handlers[eventName].Remove(handlerType);
            if (!_handlers[eventName].Any())
            {
                _handlers.Remove(eventName);
                OnEventRemoved?.Invoke(this, eventName);
            }
        }
    }
}