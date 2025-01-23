using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace EventBus;

internal class RabbitMQConnection
{
    private readonly IConnectionFactory _connectionFactory;
    private IConnection? _connection;
    private bool _disposed;

    public RabbitMQConnection(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public bool IsConnected
    {
        get
        {
            return _connection != null && _connection.IsOpen && !_disposed;
        }
    }

    public async Task<IChannel> CreateChannelAsync()
    {
        if (!IsConnected)
        {
            throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");
        }

        return await _connection!.CreateChannelAsync();
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        _connection?.Dispose();
    }

    public async Task<bool> TryConnectAsync()
    {
        _connection = await _connectionFactory.CreateConnectionAsync();

        if (IsConnected)
        {
            _connection.ConnectionShutdownAsync += OnConnectionShutdown;
            _connection.CallbackExceptionAsync += OnCallbackException;
            _connection.ConnectionBlockedAsync += OnConnectionBlocked;
            return true;
        }
        else
        {
            return false;
        }
    }

    private async Task OnCallbackException(object? sender, CallbackExceptionEventArgs e)
    {
        if (_disposed) return;
        await TryConnectAsync();
    }

    private async Task OnConnectionBlocked(object? sender, ConnectionBlockedEventArgs e)
    {
        if (_disposed) return;
        await TryConnectAsync();
    }

    private async Task OnConnectionShutdown(object? sender, ShutdownEventArgs args)
    {
        if (_disposed) return;
        await TryConnectAsync();
    }
}