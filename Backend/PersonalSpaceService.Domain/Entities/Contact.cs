using DomainCommons;
using DomainCommons.EntityStronglyIds;

namespace PersonalSpaceService.Domain.Entities
{
    public class Contact : Entity<ContactId>
    {
        public Contact(UserId userId)
        {
            UserId = userId;
            Id = ContactId.New();
        }

        public override ContactId Id { get; protected set; }

        public UserId UserId { get; private set; }
    }
}