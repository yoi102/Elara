using ChatService.Domain.Events;
using DomainCommons;
using DomainCommons.EntityStronglyIds;

namespace ChatService.Domain.Entities;

public record Conversation : AggregateRootEntity<ConversationId>
{
    public Conversation(string name, bool isGroup)
    {
        Name = name;
        IsGroup = isGroup;
        Id = ConversationId.New();
    }
    private Conversation()
    {
    }

    public override ConversationId Id { get; protected set; }
    public bool IsGroup { get; private init; }

    public string Name { get; private set; } = string.Empty;

    public void ChangeName(string value)
    {
        if (Name == value)
            return;
        Name = value;
        AddDomainEventIfAbsent(new ConversationUpdatedEvent(this));
        NotifyModified();
    }
}
