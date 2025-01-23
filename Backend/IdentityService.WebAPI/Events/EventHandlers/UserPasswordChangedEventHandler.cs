using EventBus;
using IdentityService.Domain.Events;
using IdentityService.WebAPI.Events.Args;
using MediatR;

namespace IdentityService.WebAPI.Events.EventHandlers;

public class UserPasswordChangedEventHandler : INotificationHandler<UserPasswordChangedEvent>
{
    private readonly IEventBus eventBus;

    public UserPasswordChangedEventHandler(IEventBus eventBus)
    {
        this.eventBus = eventBus;
    }

    public async Task Handle(UserPasswordChangedEvent notification, CancellationToken cancellationToken)
    {
        var arg = new UserPasswordChangedEventArgs(notification.User.Id, notification.User.UserName!);

        await eventBus.PublishAsync("UserService.User.UserPasswordReset", arg);
    }
}