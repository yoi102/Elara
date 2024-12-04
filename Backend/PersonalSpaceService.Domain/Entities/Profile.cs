using DomainCommons;
using DomainCommons.EntityStronglyIds;
using PersonalSpaceService.Domain.Events;

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




        public void ChangeDisplayName(string displayName)
        {
            if (DisplayName == displayName) return;
            DisplayName = displayName;
            this.AddDomainEventIfAbsent(new ProfileUpdatedEvent(this));

        }
    }
}