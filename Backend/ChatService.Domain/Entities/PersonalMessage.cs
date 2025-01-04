using DomainCommons.EntityStronglyIds;

namespace ChatService.Domain.Entities
{
    public class PersonalMessage : MessageBase
    {
        public PersonalMessage(UserId senderId, PersonalConversationId conversationId) : base(senderId)
        {
            ConversationId = conversationId;
            Id = MessageId.New();
        }

        public bool IsRead { get; private set; }
        public MessageId? Quote { get; private set; }
        public PersonalConversationId ConversationId { get; private set; }
    }
}