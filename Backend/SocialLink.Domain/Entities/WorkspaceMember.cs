using DomainCommons;
using Strongly;

namespace SocialLink.Domain.Entities
{
    [Strongly(converters: StronglyConverter.EfValueConverter |
                          StronglyConverter.SwaggerSchemaFilter |
                          StronglyConverter.SystemTextJson |
                          StronglyConverter.TypeConverter)]
    public partial struct WorkspaceMemberId;

    public class WorkspaceMember : Entity<WorkspaceMemberId>
    {
        public WorkspaceMember(UserId userId)
        {
            Id = WorkspaceMemberId.New();
            UserId = userId;
        }

        private WorkspaceMember()
        {
        }
        public override WorkspaceMemberId Id { get; protected set; }
        public UserId UserId { get; private set; }
    }
}