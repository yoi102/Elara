using DomainCommons;
using DomainCommons.EntityStronglyIds;

namespace SocialLink.Domain.Entities
{
    public class Workspace : AggregateRootEntity<WorkspaceId>
    {
        public Workspace(string name)
        {
            Id = WorkspaceId.New();
            Name = name;
        }

        public override WorkspaceId Id { get; protected set; }
        public ICollection<UserId> MemberIds { get; } = new HashSet<UserId>();
        public string Name { get; set; }
    }
}