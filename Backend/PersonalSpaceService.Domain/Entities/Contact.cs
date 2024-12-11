using DomainCommons;
using DomainCommons.EntityStronglyIds;
using PersonalSpaceService.Domain.Events;

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
        public string Remark { get; private set; } = null!;
        public UserId UserId { get; private set; }

        public void ChangeRemark(string remark)
        {
            Remark = remark;
            this.AddDomainEventIfAbsent(new ContactUpdatedEvent(this));
        }
    }
}