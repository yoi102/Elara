using ChatService.Domain.Events;
using DomainCommons;
using DomainCommons.EntityStronglyIds;

namespace ChatService.Domain.Entities
{
    public abstract class MessageBase : AggregateRootEntity<MessageId>
    {
        private readonly List<Uri> attachments = [];

        protected MessageBase(UserId senderId)
        {
            Id = MessageId.New();
            SenderId = senderId;
        }

        public IReadOnlyCollection<Uri> Attachments => attachments.AsReadOnly();

        public string? Content { get; private set; }
        public override MessageId Id { get; protected set; }
        public UserId SenderId { get; private set; }

        public void AddAttachment(Uri value)
        {
            attachments.Add(value);
            this.AddDomainEventIfAbsent(new MessageUpdatedEvent(this));
            NotifyModified();
        }

        public void ChangeContent(string value)
        {
            Content = value;
            this.AddDomainEventIfAbsent(new MessageUpdatedEvent(this));
            NotifyModified();
        }

        public void RemoveAttachment(Uri value)
        {
            attachments.Remove(value);
            this.AddDomainEventIfAbsent(new MessageUpdatedEvent(this));
            NotifyModified();
        }

        public override void SoftDelete()
        {
            base.SoftDelete();
            this.AddDomainEventIfAbsent(new MessageUpdatedEvent(this));
        }
    }
}