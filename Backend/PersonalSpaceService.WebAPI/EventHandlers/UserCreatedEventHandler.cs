using DomainCommons.EntityStronglyIds;
using EventBus.Attributes;
using EventBus.Handlers;
using PersonalSpaceService.Domain.Interfaces;
using PersonalSpaceService.Infrastructure;

namespace PersonalSpaceService.WebAPI.EventHandlers;

[EventName("IdentityService.UserCreated")]
public class UserCreatedEventHandler : DynamicIntegrationEventHandler
{
    private readonly IPersonalSpaceRepository repository;
    private readonly PersonalSpaceDbContext dbContext;

    public UserCreatedEventHandler(IPersonalSpaceRepository repository, PersonalSpaceDbContext dbContext)
    {
        this.repository = repository;
        this.dbContext = dbContext;
    }

    public override async Task HandleDynamic(string eventName, dynamic eventData)
    {
        if (!UserId.TryParse(eventData.UserId, out UserId userId))
        {
            return;
        }
        if (string.IsNullOrEmpty(eventData.UserName))
        {
            return;
        }

        await repository.CreateProfileAsync(userId, eventData.UserName);

        await dbContext.SaveChangesAsync();
    }
}
