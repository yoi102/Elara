using DomainCommons;
using DomainCommons.EntityStronglyIds;

namespace PersonalSpaceService.Domain.Entities
{
    public class SocialContext : Entity<SocialContextId>
    {
        public SocialContext(UserId userId)
        {
            UserId = userId;
            Id = SocialContextId.New();
            ContactIds = [];
            WorkspaceIds = [];
        }

        private SocialContext()
        {
        }

        public override SocialContextId Id { get; protected set; }
        public UserId UserId { get; private set; }

        public List<ContactId> ContactIds { get; private set; } = null!;

        public List<WorkspaceId> WorkspaceIds { get; private set; } = null!;
    }
}