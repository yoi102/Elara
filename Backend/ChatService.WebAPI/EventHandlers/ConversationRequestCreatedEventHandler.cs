using ChatService.Domain.Events;
using MediatR;

namespace ChatService.WebAPI.EventHandlers;

public class ConversationRequestCreatedEventHandler : INotificationHandler<ConversationRequestCreatedEvent>
{
    public Task Handle(ConversationRequestCreatedEvent notification, CancellationToken cancellationToken)
    {
        //todo
        return Task.CompletedTask;
    }
}
