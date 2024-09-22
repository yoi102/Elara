using System.Text.Json;

namespace EventBus.Handlers;

public abstract class JsonIntegrationEventHandler<T> : IIntegrationEventHandler
{
    public Task Handle(string eventName, string json)
    {
        var eventData = JsonSerializer.Deserialize<T>(json);
        if (eventData == null)
        {
            throw new JsonException("Failed to deserialize the JSON to the expected type.");
        }
        return HandleJson(eventName, eventData);
    }

    public abstract Task HandleJson(string eventName, T eventData);
}