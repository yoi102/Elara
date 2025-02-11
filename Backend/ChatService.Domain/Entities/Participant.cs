using ChatService.Domain.Events;
using DomainCommons;
using DomainCommons.EntityStronglyIds;

namespace ChatService.Domain.Entities;

public record Participant : Entity<ParticipantId>
{
    public Participant(ConversationId groupConversationId, UserId user, string role = "")
    {
        Id = ParticipantId.New();
        ConversationId = groupConversationId;
        UserId = user;
        Role = role;
        this.AddDomainEventIfAbsent(new ParticipantCreatedEvent(this));
    }
    private Participant()
    {
    }

    public override ParticipantId Id { get; protected set; }
    public UserId UserId { get; private set; }
    public ConversationId ConversationId { get; private set; }
    public string Role { get; private set; } = Roles.Member;

    public void ChangeRole(string value)
    {
        if (Role == value)
            return;
        Role = value;
        this.AddDomainEventIfAbsent(new ParticipantUpdatedEvent(this));
    }
}

public static class Roles
{
    public static string Empty { get; } = string.Empty;
    public static string Manager { get; } = "Manager";
    public static string Member { get; } = "Member";
    public static string Owner { get; } = "Owner";
}
