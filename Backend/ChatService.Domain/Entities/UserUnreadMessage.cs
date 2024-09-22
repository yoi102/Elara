using ChatService.Domain.Events;
using DomainCommons;
using DomainCommons.EntityStronglyIds;

namespace ChatService.Domain.Entities;
public record UserUnreadMessage : Entity<UserUnreadMessageId>
{
    public UserUnreadMessage(UserId receiverUserId, MessageId messageId)
    {
        Id = UserUnreadMessageId.New();
        UserId = receiverUserId;
        MessageId = messageId;
        AddDomainEventIfAbsent(new UserUnreadMessageCreatedEvent(this));
    }
    private UserUnreadMessage()
    {
    }
    public override UserUnreadMessageId Id { get; protected set; }
    public UserId UserId { get; private set; }
    public MessageId MessageId { get; private set; }
    public bool HasBeenRead { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public void MarkAsRead()
    {
        HasBeenRead = false;
        AddDomainEventIfAbsent(new UserUnreadMessageUpdatedEvent(this));
    }
}
