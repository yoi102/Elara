using DomainCommons;
using DomainCommons.EntityStronglyIds;

namespace ChatService.Domain.Entities
{
    public class PersonalConversation : AggregateRootEntity<PersonalConversationId>
    {
        public PersonalConversation(UserId user1Id, UserId user2Id)
        {
            User1Id = user1Id;
            User2Id = user2Id;
            Id = PersonalConversationId.New();
        }

        public override PersonalConversationId Id { get; protected set; }
        public UserId User1Id { get; private set; }
        public UserId User2Id { get; private set; }
    }
}