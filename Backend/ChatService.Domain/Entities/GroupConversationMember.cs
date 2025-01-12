using DomainCommons.EntityStronglyIds;
using DomainCommons;
using ChatService.Domain.Events;

namespace ChatService.Domain.Entities;

public record GroupConversationMember : Entity<GroupConversationMemberId>
{
    public GroupConversationMember(GroupConversationId groupConversationId, UserId user, string role = "member")
    {
        Id = GroupConversationMemberId.New();
        GroupConversationId = groupConversationId;
        UserId = user;
        Role = role;
        this.AddDomainEventIfAbsent(new GroupConversationMemberCreatedEvent(this));
    }
    private GroupConversationMember()
    {
    }

    public override GroupConversationMemberId Id { get; protected set; }
    public UserId UserId { get; private set; }
    public GroupConversationId GroupConversationId { get; private set; }
    public string Role { get; private set; } = string.Empty;

    public void ChangeRole(string value)
    {
        Role = value;
        this.AddDomainEventIfAbsent(new GroupConversationMemberUpdatedEvent(this));
    }
}
