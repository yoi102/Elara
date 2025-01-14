using ChatService.Domain.Events;
using DomainCommons;
using DomainCommons.EntityStronglyIds;

namespace ChatService.Domain.Entities;

public record GroupConversation : AggregateRootEntity<GroupConversationId>
{
    public GroupConversation(string name)
    {
        Name = name;
        Id = GroupConversationId.New();
    }
    private GroupConversation()
    {
    }

    public override GroupConversationId Id { get; protected set; }
    public string Name { get; set; } = string.Empty;

    public void ChangeName(string value)
    {
        if (Name == value)
            return;
        Name = value;
        AddDomainEventIfAbsent(new GroupConversationUpdatedEvent(this));
        NotifyModified();
    }
}
