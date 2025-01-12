using DomainCommons;
using DomainCommons.EntityStronglyIds;
using PersonalSpaceService.Domain.Events;

namespace PersonalSpaceService.Domain.Entities;

public record Contact : AggregateRootEntity<ContactId>
{
    public Contact(UserId ownerId, UserId contactId, string remark)
    {
        ContactId = contactId;
        Id = DomainCommons.EntityStronglyIds.ContactId.New();
        OwnerId = ownerId;
        Remark = remark;
    }

    private Contact()
    {
    }

    public UserId ContactId { get; private set; }
    public override ContactId Id { get; protected set; }
    public UserId OwnerId { get; private set; }
    public string Remark { get; private set; } = null!;

    public void ChangeRemark(string remark)
    {
        Remark = remark;
        this.AddDomainEventIfAbsent(new ContactUpdatedEvent(this));
    }
}
