using DomainCommons;
using DomainCommons.EntityStronglyIds;

namespace PersonalSpaceService.Domain.Entities
{
    public class Profile : AggregateRootEntity<ProfileId>
    {
        public Profile(UserId userId, string displayName)
        {
            UserId = userId;
            Id = ProfileId.New();
            DisplayName = displayName;
        }

        private Profile()
        {
        }

        public Uri? Avatar { get; private set; }
        public string DisplayName { get; private set; } = null!;
        public override ProfileId Id { get; protected set; }
        public UserId UserId { get; private set; }
    }
}