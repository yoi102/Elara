using ChatService.Domain.Events;
using DomainCommons;
using DomainCommons.EntityStronglyIds;

namespace ChatService.Domain.Entities;
public record UserUnreadMessage : Entity<UserUnreadMessageId>
{
    public UserUnreadMessage(UserId userId, MessageId messageId, ConversationId conversationId)
    {
        Id = UserUnreadMessageId.New();
        UserId = userId;
        MessageId = messageId;
        ConversationId = conversationId;
        AddDomainEventIfAbsent(new UserUnreadMessageCreatedEvent(this));
    }
    private UserUnreadMessage()
    {
    }
    public override UserUnreadMessageId Id { get; protected set; }
    public ConversationId ConversationId { get; private set; }
    public UserId UserId { get; private set; }
    public MessageId MessageId { get; private set; }
}
