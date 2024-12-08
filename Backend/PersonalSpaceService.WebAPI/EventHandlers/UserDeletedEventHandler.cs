using EventBus;

namespace PersonalSpaceService.WebAPI.EventHandlers
{
    [EventName("UserDeletedEvent")]
    public class UserDeletedEventHandler : DynamicIntegrationEventHandler
    {
        public override Task HandleDynamic(string eventName, dynamic eventData)
        {
            throw new NotImplementedException();
        }
    }
}
