using ChatService.Domain.Events;
using MediatR;

namespace ChatService.WebAPI.EventHandlers;

public class ParticipantCreatedEventHandler : INotificationHandler<ParticipantCreatedEvent>
{
    public Task Handle(ParticipantCreatedEvent notification, CancellationToken cancellationToken)
    {
        //TODO:SignalR
        return Task.CompletedTask;
    }
}
