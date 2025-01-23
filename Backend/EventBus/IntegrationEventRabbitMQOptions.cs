namespace EventBus;

public class IntegrationEventRabbitMQOptions
{
    public required string ExchangeName { get; set; }
    public required string HostName { get; set; }
    public required string Password { get; set; }
    public required string UserName { get; set; }
}