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
    public class Message : Entity<MessageId>,ISoftDelete
    {
        private Message()
        {
        }
        public Message(string content, UserId senderId, ConversationId conversationId)
        {
            Id = MessageId.New();
            SentAt = DateTimeOffset.Now;
            IsRead = false;
            Content = content;
            SenderId = senderId;
            ConversationId = conversationId;
        }

        public override MessageId Id { get; }

        public string Content { get; private set; }
        public DateTimeOffset SentAt { get; private set; }
        public UserId SenderId { get; private set; }
        public ConversationId ConversationId { get; private set; }
        public bool IsRead { get; private set; }

        public bool IsDeleted { get; private set; }

        public void MarkAsRead()
        {
            IsRead = true;
            AddDomainEvent(new MessageMarkAsRead(this));
        }

        public void SoftDelete()
        {
            IsDeleted = true;
            AddDomainEvent(new MessageDeleted(this));

        }
    }
}
