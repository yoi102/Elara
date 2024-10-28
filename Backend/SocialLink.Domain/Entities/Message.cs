using DomainCommons;
using DomainCommons.EntityStronglyIds;
using SocialLink.Domain.Events.MessageEvents;

namespace SocialLink.Domain.Entities
{
    public class Message : Entity<MessageId>
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

        public string Content { get; private set; } = string.Empty;
        public ConversationId ConversationId { get; private set; }
        public ICollection<UploadedItemId> FileIds { get; private set; } = new List<UploadedItemId>();
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

        public void ModifyMessage(string content, List<UploadedItemId> fileIds)
        {
            Content = content;
            FileIds = fileIds;
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