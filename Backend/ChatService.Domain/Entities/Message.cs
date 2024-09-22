using ChatService.Domain.Events;
using DomainCommons;
using DomainCommons.EntityStronglyIds;

namespace ChatService.Domain.Entities;

public record Message : AggregateRootEntity<MessageId>
{
    public Message(UserId senderId, ConversationId conversationId,
                    string content, MessageId? quoteMessageId = null)
    {
        ConversationId = conversationId;
        QuoteMessageId = quoteMessageId;
        Id = MessageId.New();
        SenderId = senderId;
        Content = content;
        AddDomainEvent(new MessageCreatedEvent(this));
    }

    private Message()
    {
    }

    public ConversationId ConversationId { get; set; }
    public MessageId? QuoteMessageId { get; private set; }

    public string Content { get; protected set; } = string.Empty;
    public override MessageId Id { get; protected set; }
    public UserId SenderId { get; protected set; }

    public void ChangeContent(string value)
    {
        if (Content == value)
            return;
        Content = value;
        this.AddDomainEventIfAbsent(new MessageUpdatedEvent(this));
        NotifyModified();
    }
    public void ChangeQuote(MessageId? value)
    {
        if (QuoteMessageId == value)
            return;
        QuoteMessageId = value;
        this.AddDomainEventIfAbsent(new MessageUpdatedEvent(this));
        NotifyModified();
    }

    public override void SoftDelete()
    {
        base.SoftDelete();
        this.AddDomainEventIfAbsent(new MessageUpdatedEvent(this));
    }
}
