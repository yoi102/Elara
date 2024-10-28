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
        public string Name { get; set; } = null!;
        public ICollection<ParticipantId> ParticipantIds { get; set; } = new HashSet<ParticipantId>();
        public ICollection<MessageId> MessageIds { get; set; } = new HashSet<MessageId>();
    }
}