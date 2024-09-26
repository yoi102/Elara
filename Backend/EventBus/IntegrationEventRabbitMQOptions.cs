namespace EventBus
{
    public class IntegrationEventRabbitMQOptions
    {
        public string? ExchangeName { get; set; }
        public string? HostName { get; set; }
        public string? Password { get; set; }
        public string? UserName { get; set; }
    }
}