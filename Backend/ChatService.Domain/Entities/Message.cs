using DomainCommons.EntityStronglyIds;

namespace ChatService.Domain.Entities;

public record Message : BaseMessage
{
    public Message(UserId senderId, ConversationId conversationId,
                    string content, Uri[] attachments, MessageId? quoteMessageId = null) : base(senderId, content, attachments)
    {
        ConversationId = conversationId;
        QuoteMessageId = quoteMessageId;
    }

    private Message()
    {
    }

    public ConversationId ConversationId { get; set; }
    public MessageId? QuoteMessageId { get; private set; }
}
