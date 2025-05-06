using ChatService.Domain.Events;
using MediatR;

namespace ChatService.WebAPI.EventHandlers;

public class ParticipantUpdatedEventHandler : INotificationHandler<ParticipantUpdatedEvent>
{
    public Task Handle(ParticipantUpdatedEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
