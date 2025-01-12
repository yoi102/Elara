using DomainCommons.EntityStronglyIds;
using EventBus.Attributes;
using EventBus.Handlers;
using PersonalSpaceService.Domain.Interfaces;

namespace PersonalSpaceService.WebAPI.EventHandlers;

[EventName("IdentityService.UserCreated")]
public class UserCreatedEventHandler : DynamicIntegrationEventHandler
{
    private readonly IPersonalSpaceRepository repository;

    public UserCreatedEventHandler(IPersonalSpaceRepository repository)
    {
        this.repository = repository;
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
    }
}
