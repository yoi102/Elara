using EventBus;
using IdentityService.Domain.Events;
using IdentityService.WebAPI.Events.Args;
using MediatR;

namespace IdentityService.WebAPI.Events.EventHandlers;

public class UserCreatedEventHandler : INotificationHandler<UserCreatedEvent>
{
    private readonly IEventBus eventBus;

    public UserCreatedEventHandler(IEventBus eventBus)
    {

        this.eventBus = eventBus;
    }
    public async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
    {
        var arg = new UserCreatedEventArgs(notification.User.Id, notification.User.UserName!);

        await eventBus.PublishAsync("UserService.User.Created", arg);

    }
}