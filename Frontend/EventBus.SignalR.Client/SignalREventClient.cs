using Microsoft.AspNetCore.SignalR.Client;

namespace EventBus.SignalR.Client;

internal class SignalREventClient : ISignalREventClient
{
    public SignalREventClient(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    private HubConnection? _connection;
    private readonly IServiceProvider _serviceProvider;

    public void Build(string url)
    {
        if (_connection is not null)
        {
            throw new InvalidOperationException("SignalREventClient has already been built. Please call DisposeAsync() before calling Build() again.");
        }

        var connection = new HubConnectionBuilder()
                       .WithUrl(url)
                       .WithAutomaticReconnect()
                       .Build();

        EventHandlerRegistrar.MapHandlers(connection, _serviceProvider);

        _connection = connection;
    }

    public async Task DisposeAsync()
    {
        if (_connection is not null)
        {
            await _connection.DisposeAsync();
        }
        _connection = null;
    }
}
