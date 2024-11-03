using DomainCommons;
using SocialLink.Domain.Enums;
using Strongly;

namespace SocialLink.Domain.Entities
{
    [Strongly(converters: StronglyConverter.EfValueConverter |
            StronglyConverter.SwaggerSchemaFilter |
            StronglyConverter.SystemTextJson |
            StronglyConverter.TypeConverter)]
    public partial struct ContactInvitationId;

    public class ContactInvitation : Entity<ContactInvitationId>, IHasCreationTime
    {
        public ContactInvitation(UserId inviterId, UserId inviteeId)
        {
            Id = ContactInvitationId.New();
            InviterId = inviterId;
            InviteeId = inviteeId;
            Status = InvitationStatus.Pending;
        }

        private ContactInvitation()
        {
        }

        public DateTimeOffset CreationTime { get; private set; }
        public override ContactInvitationId Id { get; protected set; }
        public UserId InviteeId { get; private set; }
        public UserId InviterId { get; private set; }
        public DateTime? RespondedTime { get; private set; }
        public InvitationStatus Status { get; private set; }
    }
}