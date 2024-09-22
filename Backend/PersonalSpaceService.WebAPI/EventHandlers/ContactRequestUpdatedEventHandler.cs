using MediatR;
using PersonalSpaceService.Domain.Events;
using PersonalSpaceService.Infrastructure;

namespace PersonalSpaceService.WebAPI.EventHandlers;

public class ContactRequestUpdatedEventHandler : INotificationHandler<ContactRequestUpdatedEvent>
{
    private readonly PersonalSpaceDbContext dbContext;

    public ContactRequestUpdatedEventHandler(PersonalSpaceDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    public Task Handle(ContactRequestUpdatedEvent notification, CancellationToken cancellationToken)
    {
        //todo
        return Task.CompletedTask;
    }
}
