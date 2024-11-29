using DomainCommons;
using Strongly;

namespace SocialLink.Domain.Entities
{
    [Strongly(converters: StronglyConverter.EfValueConverter |
                          StronglyConverter.SwaggerSchemaFilter |
                          StronglyConverter.SystemTextJson |
                          StronglyConverter.TypeConverter)]
    public partial struct WorkspaceId;

    public class Workspace : AggregateRootEntity<WorkspaceId>
    {
        public Workspace(string name)
        {
            Id = WorkspaceId.New();
            Name = name;
        }

        public ICollection<WorkspaceConversationId> ConversationIds { get; } = new HashSet<WorkspaceConversationId>();
        public override WorkspaceId Id { get; protected set; }
        public ICollection<WorkspaceMemberId> MemberIds { get; } = new HashSet<WorkspaceMemberId>();
        public string Name { get; set; }
        public ICollection<WorkspaceInvitationId> SentWorkspaceInvitationIds { get; private set; } = new HashSet<WorkspaceInvitationId>();
    }
}