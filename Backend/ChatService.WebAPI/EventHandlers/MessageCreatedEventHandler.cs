using ChatService.Domain.Events;
using MediatR;

namespace ChatService.WebAPI.EventHandlers;

public class MessageCreatedEventHandler : INotificationHandler<MessageCreatedEvent>
{
    public Task Handle(MessageCreatedEvent notification, CancellationToken cancellationToken)
    {
        //TODO:SignalR
        return Task.CompletedTask;
    }
}
