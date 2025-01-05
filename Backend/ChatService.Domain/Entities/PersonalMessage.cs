using DomainCommons.EntityStronglyIds;

namespace ChatService.Domain.Entities
{
    public record PersonalMessage : MessageBase
    {
        public PersonalMessage(UserId senderId, PersonalConversationId personalConversationId) : base(senderId)
        {
            this.PersonalConversationId = personalConversationId;
            Id = MessageId.New();
        }
        private PersonalMessage()
        {

        }

        public bool IsRead { get; private set; }
        public MessageId? Quote { get; private set; }
        public PersonalConversationId PersonalConversationId { get; private set; }
    }
}
