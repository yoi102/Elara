namespace EventBus.SignalR.Client;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class EventNameAttribute : Attribute
{
    public EventNameAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; init; }
}
