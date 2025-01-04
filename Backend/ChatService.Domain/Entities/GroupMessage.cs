using DomainCommons.EntityStronglyIds;

namespace ChatService.Domain.Entities
{
    public class GroupMessage : MessageBase
    {
        public GroupMessage(UserId senderId, GroupConversationId groupConversationId) : base(senderId)
        {
            GroupConversationId = groupConversationId;
        }

        public GroupConversationId GroupConversationId { get; set; }
        public MessageId? Quote { get; private set; } 
    }
}