using MediatR;
using PersonalSpaceService.Domain.Events;

namespace PersonalSpaceService.WebAPI.EventHandlers;

public class ContactRequestUpdatedEventHandler : INotificationHandler<ContactRequestUpdatedEvent>
{
    public Task Handle(ContactRequestUpdatedEvent notification, CancellationToken cancellationToken)
    {
        //todo
        throw new NotImplementedException();
    }
}
