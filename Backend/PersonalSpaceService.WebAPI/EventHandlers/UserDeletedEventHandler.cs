using DomainCommons.EntityStronglyIds;
using EventBus.Attributes;
using EventBus.Handlers;
using PersonalSpaceService.Domain.Interfaces;
using PersonalSpaceService.Infrastructure;

namespace PersonalSpaceService.WebAPI.EventHandlers;

[EventName("IdentityService.UserDeleted")]
public class UserDeletedEventHandler : DynamicIntegrationEventHandler
{
    private readonly IPersonalSpaceRepository repository;
    private readonly PersonalSpaceDbContext dbContext;

    public UserDeletedEventHandler(IPersonalSpaceRepository repository, PersonalSpaceDbContext dbContext)
    {
        this.repository = repository;
        this.dbContext = dbContext;
    }

    public override async Task HandleDynamic(string eventName, dynamic eventData)
    {
        if (!UserId.TryParse(eventData.UserId, out UserId userId))
            return;

        var userProfile = await repository.FindProfileByUserIdAsync(userId);

        if (userProfile is null)
            return;
        dbContext.Profiles.Remove(userProfile);

        await dbContext.SaveChangesAsync();
    }
}
