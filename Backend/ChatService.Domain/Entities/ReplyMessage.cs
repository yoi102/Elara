using DomainCommons;
using DomainCommons.EntityStronglyIds;

namespace ChatService.Domain.Entities
{
    public class ReplyMessage : MessageBase
    {
        public ReplyMessage(UserId senderId, MessageId messageId) : base(senderId)
        {
            MessageId = messageId;
        }

        public MessageId MessageId { get; private set; }
    }
}