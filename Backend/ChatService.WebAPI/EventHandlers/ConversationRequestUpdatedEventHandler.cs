using ChatService.Domain.Events;
using MediatR;

namespace ChatService.WebAPI.EventHandlers;

public class ConversationRequestUpdatedEventHandler : INotificationHandler<ConversationRequestUpdatedEvent>
{
    public Task Handle(ConversationRequestUpdatedEvent notification, CancellationToken cancellationToken)
    {
        //todo
        return Task.CompletedTask;
    }
}
