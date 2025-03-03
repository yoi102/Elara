using DomainCommons;
using DomainCommons.EntityStronglyIds;
using PersonalSpaceService.Domain.Events;

namespace PersonalSpaceService.Domain.Entities;

public enum ContactRequestStatus
{
    Pending,
    Accepted,
    Rejected
}

public record ContactRequest : AggregateRootEntity<ContactRequestId>
{
    private ContactRequest()
    {
    }
    public ContactRequest(UserId senderId, UserId receiverId)
    {
        Id = ContactRequestId.New();
        SenderId = senderId;
        ReceiverId = receiverId;
        Status = ContactRequestStatus.Pending;
    }

    public override ContactRequestId Id { get; protected set; }
    public UserId SenderId { get; }
    public UserId ReceiverId { get; }
    public ContactRequestStatus Status { get; private set; }

    public void UpdateStatus(ContactRequestStatus value)
    {
        Status = value;
        this.AddDomainEventIfAbsent(new ContactRequestUpdateEvent(this));
    }
}
