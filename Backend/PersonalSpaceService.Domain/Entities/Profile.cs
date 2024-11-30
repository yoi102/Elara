using DomainCommons;
using DomainCommons.EntityStronglyIds;

namespace PersonalSpaceService.Domain.Entities
{
    public class Profile : AggregateRootEntity<ProfileId>
    {
        public Profile(UserId userId)
        {
            UserId = userId;
            Id = ProfileId.New();
        }

        private Profile()
        {
        }

        public Uri? Avatar { get; private set; }
        public string? DisplayName { get; private set; }
        public override ProfileId Id { get; protected set; }
        public UserId UserId { get; private set; }
    }
}