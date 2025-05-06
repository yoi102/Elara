using ChatService.Domain.Events;
using MediatR;

namespace ChatService.WebAPI.EventHandlers;

public class MessageUpdatedEventHandler : INotificationHandler<MessageUpdatedEvent>
{
    public Task Handle(MessageUpdatedEvent notification, CancellationToken cancellationToken)
    {
        //TODO:SignalR
        return Task.CompletedTask;
    }
}
