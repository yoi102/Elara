using DomainCommons;
using DomainCommons.EntityStronglyIds;

namespace SocialLink.Domain.Entities
{
    public class Reaction : Entity<ReactionId>
    {
        public Reaction(string emoji, MessageId messageId, UserId userId)
        {
            Id = ReactionId.New();
            Emoji = emoji;
            MessageId = messageId;
            UserId = userId;
        }

        private Reaction()
        {
        }
        public string Emoji { get; private set; } = null!;
        public override ReactionId Id { get; protected set; }
        public MessageId MessageId { get; private set; }
        public UserId UserId { get; private set; }
    }
}