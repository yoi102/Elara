using ChatService.Domain.Events;
using DomainCommons.EntityStronglyIds;

namespace ChatService.Domain.Entities
{
    public record GroupMessage : MessageBase
    {
        public GroupMessage(UserId senderId, GroupConversationId groupConversationId, 
                        string content, Uri[] attachments) : base(senderId,  content, attachments)
        {
            GroupConversationId = groupConversationId;
        }

        private GroupMessage()
        {
        }


        public GroupConversationId GroupConversationId { get; set; }
        public MessageId? Quote { get; private set; } 





    }
}
