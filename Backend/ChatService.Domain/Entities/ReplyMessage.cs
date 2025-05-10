using ChatService.Domain.Events;
using DomainCommons;
using DomainCommons.EntityStronglyIds;

namespace ChatService.Domain.Entities;

public record ReplyMessage : AggregateRootEntity<ReplyMessageId>
{
    public ReplyMessage(MessageId repliedMessageId, MessageId originalMessageId)
    {
        RepliedMessageId = repliedMessageId;
        OriginalMessageId = originalMessageId;
        Id = ReplyMessageId.New();
        AddDomainEventIfAbsent(new ReplyMessageCreatedEvent(this));
    }
    private ReplyMessage()
    {
    }
    public MessageId RepliedMessageId { get; private set; }//被回复的消息
    public MessageId OriginalMessageId { get; private set; }//回复的消息
    public override ReplyMessageId Id { get; protected set; }
}
