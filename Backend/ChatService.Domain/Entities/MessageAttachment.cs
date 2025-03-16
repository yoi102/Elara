using ChatService.Domain.Events;
using DomainCommons;
using DomainCommons.EntityStronglyIds;

namespace ChatService.Domain.Entities;
public record MessageAttachment : Entity<MessageAttachmentId>
{
    public MessageAttachment(MessageId messageId, UploadedItemId uploadedItemId)
    {
        MessageId = messageId;
        UploadedItemId = uploadedItemId;
        Id = MessageAttachmentId.New();
        AddDomainEventIfAbsent(new MessageAttachmentCreatedEvent(this));
    }
    private MessageAttachment()
    {
    }

    public UploadedItemId UploadedItemId { get; private set; }
    public MessageId MessageId { get; private set; }
    public override MessageAttachmentId Id { get; protected set; }
}
