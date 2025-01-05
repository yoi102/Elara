using DomainCommons.EntityStronglyIds;

namespace ChatService.Domain.Entities
{
    public record ReplyMessage : MessageBase
    {
        public ReplyMessage(UserId senderId, MessageId messageId, 
                        string content, Uri[] attachments) : base(senderId, content, attachments)
        {
            MessageId = messageId;
        }
        private ReplyMessage()
        {
                
        }
        public MessageId MessageId { get; private set; }
    }
}
