using MediatR;
using PersonalSpaceService.Domain.Events;
using PersonalSpaceService.Infrastructure;

namespace PersonalSpaceService.WebAPI.EventHandlers;

public class ContactRequestCreatedEventHandler : INotificationHandler<ContactRequestCreatedEvent>
{
    private readonly PersonalSpaceDbContext dbContext;

    public ContactRequestCreatedEventHandler(PersonalSpaceDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    public Task Handle(ContactRequestCreatedEvent notification, CancellationToken cancellationToken)
    {
        //todo
        return Task.CompletedTask;
    }
}
