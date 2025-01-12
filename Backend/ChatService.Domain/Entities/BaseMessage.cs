using ChatService.Domain.Events;
using DomainCommons;
using DomainCommons.EntityStronglyIds;

namespace ChatService.Domain.Entities;

public abstract record BaseMessage : AggregateRootEntity<MessageId>
{
    protected List<Uri> attachments = [];

    protected BaseMessage(UserId senderId, string content, Uri[] attachments)
    {
        Id = MessageId.New();
        SenderId = senderId;
        Content = content;
        this.attachments = [.. attachments];
        AddDomainEvent(new MessageCreatedEvent(this));
    }

    protected BaseMessage()
    { }

    public IReadOnlyCollection<Uri> Attachments => attachments.AsReadOnly();

    public string Content { get; protected set; } = string.Empty;
    public override MessageId Id { get; protected set; }
    public UserId SenderId { get; protected set; }

    public void ChangeAttachments(Uri[] value)
    {
        if (attachments.SequenceEqual(value))
        {
            return;
        }
        this.attachments = [.. value];
        this.AddDomainEventIfAbsent(new MessageUpdatedEvent(this));
        NotifyModified();
    }

    public void ChangeContent(string value)
    {
        Content = value;
        this.AddDomainEventIfAbsent(new MessageUpdatedEvent(this));
        NotifyModified();
    }

    public override void SoftDelete()
    {
        base.SoftDelete();
        this.AddDomainEventIfAbsent(new MessageUpdatedEvent(this));
    }
}
