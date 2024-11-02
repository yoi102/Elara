using DomainCommons;
using SocialLink.Domain.Enums;
using Strongly;

namespace SocialLink.Domain.Entities
{
    [Strongly(converters: StronglyConverter.EfValueConverter |
                      StronglyConverter.SwaggerSchemaFilter |
                      StronglyConverter.SystemTextJson |
                      StronglyConverter.TypeConverter)]
    public partial struct ParticipantId;

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