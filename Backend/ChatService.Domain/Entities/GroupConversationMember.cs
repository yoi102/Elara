using ChatService.Domain.Events;
using DomainCommons;
using DomainCommons.EntityStronglyIds;
using System.Xml.Linq;

namespace ChatService.Domain.Entities;

public record GroupConversationMember : Entity<GroupConversationMemberId>
{
    public GroupConversationMember(GroupConversationId groupConversationId, UserId user, string role = "Member")
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
    public string Role { get; private set; } = Roles.Member;

    public void ChangeRole(string value)
    {
        if (Role == value)
            return;
        Role = value;
        this.AddDomainEventIfAbsent(new GroupConversationMemberUpdatedEvent(this));
    }
}

public static class Roles
{
    public static string Manager { get; } = "Manager";
    public static string Member { get; } = "Member";
    public static string Owner { get; } = "Owner";
}
