using DomainCommons;
using SocialLink.Domain.Enums;
using Strongly;

namespace SocialLink.Domain.Entities
{
    [Strongly(converters: StronglyConverter.EfValueConverter |
                  StronglyConverter.SwaggerSchemaFilter |
                  StronglyConverter.SystemTextJson |
                  StronglyConverter.TypeConverter)]
    public partial struct WorkspaceInvitationId;

    public class WorkspaceInvitation : Entity<WorkspaceInvitationId>, IHasCreationTime
    {
        public WorkspaceInvitation(UserId inviteeId, UserId inviterId, WorkspaceId workspaceId)
        {
            CreationTime = DateTime.Now;
            Id = WorkspaceInvitationId.New();
            InviteeId = inviteeId;
            InviterId = inviterId;
            Status = InvitationStatus.Pending;
            WorkspaceId = workspaceId;
        }

        private WorkspaceInvitation()
        {
        }

        public DateTimeOffset CreationTime { get; protected set; }
        public override WorkspaceInvitationId Id { get; protected set; }
        public UserId InviteeId { get; set; }
        public UserId InviterId { get; set; }
        public DateTime? RespondedTime { get; set; }
        public InvitationStatus Status { get; set; }
        public WorkspaceId WorkspaceId { get; set; }
    }
}