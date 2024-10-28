using DomainCommons;
using DomainCommons.EntityStronglyIds;
using SocialLink.Domain.Enums;

namespace SocialLink.Domain.Entities
{
    public class Participant : Entity<ParticipantId>
    {
        public Participant(UserId userId, ParticipantRole role)
        {
            Id = ParticipantId.New();
            UserId = userId;
            Role = role;
        }

        private Participant()
        {
        }
        public override ParticipantId Id { get; protected set; }
        public bool IsMuted { get; private set; }
        public ParticipantRole Role { get; private set; }
        public UserId UserId { get; private set; }
    }
}