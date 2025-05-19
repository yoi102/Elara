namespace EventBus.SignalR.Client;

public interface ISignalREventClient
{
    void Build(string url);

    Task DisposeAsync();
}
