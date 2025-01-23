using EventBus;
using IdentityService.Domain.Events;
using IdentityService.WebAPI.Events.Args;
using MediatR;

namespace IdentityService.WebAPI.Events.EventHandlers;

public class UserDeletedEventHandler : INotificationHandler<UserDeletedEvent>
{
    private readonly IEventBus eventBus;

    public UserDeletedEventHandler(IEventBus eventBus)
    {
        this.eventBus = eventBus;
    }

    public async Task Handle(UserDeletedEvent notification, CancellationToken cancellationToken)
    {
        var arg = new UserDeletedEventArgs(notification.User.Id, notification.User.UserName!);
        await eventBus.PublishAsync("UserService.User.Deleted", arg);

    }
}