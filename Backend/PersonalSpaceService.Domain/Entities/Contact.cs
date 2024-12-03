using DomainCommons;
using DomainCommons.EntityStronglyIds;

namespace PersonalSpaceService.Domain.Entities
{
    public class Contact : Entity<ContactId>
    {
        public Contact(UserId userId, string remark)
        {
            UserId = userId;
            Id = ContactId.New();
            Remark = remark;
        }
        private Contact()
        {
            
        }

        public override ContactId Id { get; protected set; }

        public UserId UserId { get; private set; }
        public string Remark { get; private set; } = null!;
    }
}