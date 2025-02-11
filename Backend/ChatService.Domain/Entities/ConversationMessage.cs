using DomainCommons.EntityStronglyIds;

namespace ChatService.Domain.Entities;

public record ConversationMessage : BaseMessage
{
    public ConversationMessage(UserId senderId, ConversationId conversationId,
                    string content, Uri[] attachments) : base(senderId, content, attachments)
    {
        ConversationId = conversationId;
    }

    private ConversationMessage()
    {
    }

    public ConversationId ConversationId { get; set; }
    public MessageId? Quote { get; private set; }
}
