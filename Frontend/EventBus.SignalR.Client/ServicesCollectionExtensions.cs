using Commons.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EventBus.SignalR.Client;

public static class ServicesCollectionExtensions
{
    public static readonly Dictionary<string, List<Type>> NonGenericHandlersByEvent = [];

    public static readonly Dictionary<string, (Type EventType, List<Type> HandlerTypes)> GenericHandlersByEvent = [];

    public static IServiceCollection AddEventBus(this IServiceCollection services)
    {
        var assemblies = ReflectionHelper.GetAllReferencedAssemblies();

        foreach (var assembly in assemblies)
        {
            ScanAllEventHandlers(assembly);
        }

        foreach (var type in NonGenericHandlersByEvent.SelectMany(x => x.Value))
        {
            services.AddTransient(type);
        }
        foreach (var type in GenericHandlersByEvent.Select(x => x.Value).SelectMany(x => x.HandlerTypes))
        {
            services.AddTransient(type);
        }
        services.AddSingleton<ISignalREventClient, SignalREventClient>();
        return services;
    }

    public static void ScanAllEventHandlers(Assembly assembly)
    {
        var allTypes = assembly.GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface)
            .ToList();

        foreach (var type in allTypes)
        {
            var attr = type.GetCustomAttribute<EventNameAttribute>(inherit: false);
            if (attr is null)
                continue;

            var eventName = attr.Name;
            var eventType = (Type?)null;

            foreach (var interfaceType in type.GetInterfaces())
            {
                if (interfaceType == typeof(IEventHandler))
                {
                    // ⚠️ 冲突检测
                    if (GenericHandlersByEvent.ContainsKey(eventName))
                        throw new InvalidOperationException($"Conflict: EventName '{eventName}' is already used in GenericHandlers, cannot register non-generic handler: {type.FullName}");

                    if (!NonGenericHandlersByEvent.TryGetValue(eventName, out var handlers))
                    {
                        handlers = new List<Type>();
                        NonGenericHandlersByEvent[eventName] = handlers;
                    }

                    handlers.Add(type);
                }
                else if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IEventHandler<>))
                {
                    eventType = interfaceType.GetGenericArguments()[0];

                    // ⚠️ 冲突检测
                    if (NonGenericHandlersByEvent.ContainsKey(eventName))
                        throw new InvalidOperationException($"Conflict: EventName '{eventName}' is already used in NonGenericHandlers, cannot register generic handler: {type.FullName}");

                    if (!GenericHandlersByEvent.TryGetValue(eventName, out var data))
                    {
                        data = (eventType, new List<Type>());
                        GenericHandlersByEvent[eventName] = data;
                    }
                    else
                    {
                        // ⚠️ 同一个 EventName 的泛型类型必须一致
                        if (data.EventType != eventType)
                            throw new InvalidOperationException($"Type mismatch: EventName '{eventName}' is already mapped to type '{data.EventType}', but '{type.FullName}' uses '{eventType}'");
                    }

                    data.HandlerTypes.Add(type);
                }
            }
        }
    }
}
