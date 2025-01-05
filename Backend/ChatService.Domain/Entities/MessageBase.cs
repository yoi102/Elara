using ChatService.Domain.Events;
using DomainCommons;
using DomainCommons.EntityStronglyIds;

namespace ChatService.Domain.Entities
{
    public abstract record MessageBase : AggregateRootEntity<MessageId>
    {
        private List<Uri> attachments = [];

        protected MessageBase(UserId senderId)
        {
            Id = MessageId.New();
            SenderId = senderId;
            this.attachments = attachments.ToList();
        }

        protected MessageBase()
        { }

        public IReadOnlyCollection<Uri> Attachments => attachments.AsReadOnly();

        public string? Content { get; private set; }
        public override MessageId Id { get; protected set; }
        public UserId SenderId { get; private set; }

        public void ChangeAttachments(Uri[] value)
        {
            this.attachments = value.ToList();
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
}
