using EventBus;
using EventBus.Attributes;
using EventBus.Handlers;

namespace PersonalSpaceService.WebAPI.EventHandlers
{
    [EventName("UserService.User.Created")]
    public class UserCreatedEventHandler : DynamicIntegrationEventHandler
    {
        public override Task HandleDynamic(string eventName, dynamic eventData)
        {
            throw new NotImplementedException();
        }
    }
}
