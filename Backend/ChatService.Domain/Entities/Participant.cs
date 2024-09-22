using ChatService.Domain.Events;
using DomainCommons;
using DomainCommons.EntityStronglyIds;

namespace ChatService.Domain.Entities;

public record Participant : Entity<ParticipantId>
{
    public Participant(ConversationId conversationId, UserId user, string role = "")
    {
        Id = ParticipantId.New();
        ConversationId = conversationId;
        UserId = user;
        Role = role;
        AddDomainEventIfAbsent(new ParticipantCreatedEvent(this));
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
    public const string Empty = "";
    public const string Manager = "Manager";
    public const string Member = "Member";
    public const string Owner = "Owner";
}
