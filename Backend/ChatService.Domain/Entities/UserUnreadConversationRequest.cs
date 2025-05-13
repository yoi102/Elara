using ChatService.Domain.Events;
using DomainCommons;
using DomainCommons.EntityStronglyIds;

namespace ChatService.Domain.Entities;
public record UserUnreadConversationRequest : Entity<UserUnreadConversationRequestId>
{
    public UserUnreadConversationRequest(UserId userId, ConversationRequestId conversationRequestId)
    {
        Id = UserUnreadConversationRequestId.New();
        ConversationRequestId = conversationRequestId;
        AddDomainEventIfAbsent(new UserUnreadConversationRequestCreatedEvent(this));
    }
    private UserUnreadConversationRequest()
    {
    }
    public ConversationRequestId ConversationRequestId { get; private set; }
    public UserId UserId { get; private set; }

    public override UserUnreadConversationRequestId Id { get; protected set; }
}
