using DomainCommons;
using SocialLink.Domain.Events.MessageEvents;
using Strongly;

namespace SocialLink.Domain.Entities
{
    [Strongly(converters: StronglyConverter.EfValueConverter |
                          StronglyConverter.SwaggerSchemaFilter |
                          StronglyConverter.SystemTextJson |
                          StronglyConverter.TypeConverter)]
    public partial struct MessageId;

    public class Message : AggregateRootEntity<MessageId>
    {
        public Message(string content, UserId senderId, ConversationId conversationId)
        {
            Id = MessageId.New();
            SentAt = DateTimeOffset.Now;
            IsRead = false;
            Content = content;
            SenderId = senderId;
            ConversationId = conversationId;
        }

        private Message()
        {
        }

        public ICollection<AttachmentId> AttachmentIds { get; private set; } = new List<AttachmentId>();
        public string Content { get; private set; } = string.Empty;
        public ConversationId ConversationId { get; private set; }
        public override MessageId Id { get; protected set; }
        public bool IsModified { get; private set; }
        public bool IsRead { get; private set; }
        public bool IsRecalled { get; private set; }
        public MessageId? ReferencedMessageId { get; private set; }
        public UserId SenderId { get; private set; }
        public DateTimeOffset SentAt { get; private set; }

        public void MarkAsRead()
        {
            IsRead = true;
            AddDomainEvent(new MessageMarkAsRead(this));
        }

        public void ModifyMessage(string content, List<AttachmentId> attachmentId)
        {
            Content = content;
            AttachmentIds = attachmentId;
            IsModified = true;
            AddDomainEvent(new MessageContentChanged(this));
        }

        public void Recall()
        {
            IsRecalled = true;
            AddDomainEvent(new MessageRecalled(this));
        }
    }
}