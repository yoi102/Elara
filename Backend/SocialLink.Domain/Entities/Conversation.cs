using DomainCommons;
using DomainCommons.EntityStronglyIds;

namespace SocialLink.Domain.Entities
{

    public class Conversation : Entity<ConversationId>, IHasCreationTime
    {
        public Conversation(string name)
        {
            Name = name;
            CreationTime = DateTimeOffset.Now;
        }

        private Conversation()
        {
        }
        public DateTimeOffset CreationTime { get; private set; }
        public override ConversationId Id { get; protected set; }
        public MessageId LastMessageId { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<UserId> ParticipantIds { get; set; } = new HashSet<UserId>();
    }
}