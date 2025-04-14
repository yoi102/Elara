using ChatService.Domain.Events;
using MediatR;

namespace ChatService.WebAPI.EventHandlers;

public class UserUnreadMessageCreatedEventHandler : INotificationHandler<UserUnreadMessageCreatedEvent>
{
    public Task Handle(UserUnreadMessageCreatedEvent notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
