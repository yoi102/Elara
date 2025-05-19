namespace EventBus.SignalR.Client;

public interface IEventHandler
{
    Task HandleAsync();
}

//public interface IEventHandlerObject
//{
//    Task HandleAsync(object eventData);

//}
public interface IEventHandler<T>
{
    Task HandleAsync(T eventData);
}
