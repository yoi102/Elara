using DomainCommons.EntityStronglyIds;

namespace ChatService.Domain.Entities;

public record Message : BaseMessage
{
    public Message(UserId senderId, ConversationId conversationId,
                    string content, Uri[] attachments) : base(senderId, content, attachments)
    {
        ConversationId = conversationId;
    }

    private Message()
    {
    }

    public ConversationId ConversationId { get; set; }
    public MessageId? Quote { get; private set; }
}
