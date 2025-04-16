using MediatR;
using PersonalSpaceService.Domain.Events;

namespace PersonalSpaceService.WebAPI.EventHandlers;

public class ContactRequestCreatedEventHandler : INotificationHandler<ContactRequestCreatedEvent>
{
    public Task Handle(ContactRequestCreatedEvent notification, CancellationToken cancellationToken)
    {
        //todo
        return Task.CompletedTask;
    }
}
