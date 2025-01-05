using DomainCommons.EntityStronglyIds;

namespace ChatService.Domain.Entities
{
    public record GroupMessage : MessageBase
    {
        public GroupMessage(UserId senderId, GroupConversationId groupConversationId) : base(senderId)
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
