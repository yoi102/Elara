using ChatService.Domain.Events;
using DomainCommons;
using DomainCommons.EntityStronglyIds;
using DomainCommons.Enums;

namespace ChatService.Domain.Entities;

public record ConversationRequest : AggregateRootEntity<ConversationRequestId>
{
    private ConversationRequest()
    {
    }
    public ConversationRequest(UserId senderId, UserId receiverId, ConversationId conversationId)
    {
        Id = ConversationRequestId.New();
        SenderId = senderId;
        ReceiverId = receiverId;
        ConversationId = conversationId;
        Status = RequestStatus.Pending;
        this.AddDomainEventIfAbsent(new ConversationRequestCreatedEvent(this));
    }

    public override ConversationRequestId Id { get; protected set; }
    public ConversationId ConversationId { get; }
    public UserId SenderId { get; }
    public UserId ReceiverId { get; }
    public RequestStatus Status { get; private set; }

    public void UpdateStatus(RequestStatus value)
    {
        Status = value;
        this.AddDomainEventIfAbsent(new ConversationRequestUpdatedEvent(this));
    }
}
