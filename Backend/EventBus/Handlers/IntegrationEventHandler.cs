namespace EventBus.Handlers;

public abstract class IntegrationEventHandler : IIntegrationEventHandler
{
    public abstract Task Handle(string eventName, string eventData);
}