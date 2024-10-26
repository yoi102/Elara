using DomainCommons;
using DomainCommons.EntityStronglyIds;

namespace SocialLink.Domain.Entities
{


    public class Reaction : Entity<ReactionId>
    {
        private Reaction()
        {
        }
        public Reaction(string emoji, MessageId messageId, UserId userId)
        {
            Id = ReactionId.New();
            this.Emoji = emoji;
            this.MessageId = messageId;
            this.UserId = userId;
        }

        public override ReactionId Id { get; protected set; }
        public string Emoji { get; private set; } = null!;
        public MessageId MessageId { get; private set; }
        public UserId UserId { get; private set; }
    }
}
