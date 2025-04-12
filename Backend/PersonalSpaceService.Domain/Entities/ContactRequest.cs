using DomainCommons;
using DomainCommons.EntityStronglyIds;
using DomainCommons.Enums;
using PersonalSpaceService.Domain.Events;

namespace PersonalSpaceService.Domain.Entities;

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
        Status = RequestStatus.Pending;
        this.AddDomainEventIfAbsent(new ContactRequestCreatedEvent(this));
    }

    public override ContactRequestId Id { get; protected set; }
    public UserId SenderId { get; }
    public UserId ReceiverId { get; }
    public RequestStatus Status { get; private set; }

    public void UpdateStatus(RequestStatus value)
    {
        Status = value;
        this.AddDomainEventIfAbsent(new ContactRequestUpdatedEvent(this));
    }
}
