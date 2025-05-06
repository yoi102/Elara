using ChatService.Domain.Events;
using MediatR;

namespace ChatService.WebAPI.EventHandlers;

public class ConversationRequestCreatedEventHandler : INotificationHandler<ConversationRequestCreatedEvent>
{
    public Task Handle(ConversationRequestCreatedEvent notification, CancellationToken cancellationToken)
    {
        //TODO:SignalR
        return Task.CompletedTask;
    }
}
