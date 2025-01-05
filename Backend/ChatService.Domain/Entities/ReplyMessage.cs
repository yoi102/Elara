using DomainCommons.EntityStronglyIds;

namespace ChatService.Domain.Entities
{
    public record ReplyMessage : MessageBase
    {
        public ReplyMessage(UserId senderId, MessageId messageId) : base(senderId)
        {
            MessageId = messageId;
        }
        private ReplyMessage()
        {
                
        }
        public MessageId MessageId { get; private set; }
    }
}
