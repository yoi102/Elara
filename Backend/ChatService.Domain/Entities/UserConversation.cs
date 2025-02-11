using DomainCommons;
using DomainCommons.EntityStronglyIds;

namespace ChatService.Domain.Entities;

public record UserConversation : AggregateRootEntity<UserConversationId>
{
    public UserConversation(UserId userId, ConversationId conversationId)
    {
        UserId = userId;
        ConversationId = conversationId;
        Id = UserConversationId.New();
    }
    private UserConversation()
    {
    }

    public ConversationId ConversationId { get; private set; }
    public UserId UserId { get; private set; }
    public override UserConversationId Id { get; protected set; }
}
