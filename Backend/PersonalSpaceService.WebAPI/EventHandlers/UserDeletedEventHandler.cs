using EventBus;

namespace PersonalSpaceService.WebAPI.EventHandlers
{
    [EventName("UserService.User.Deleted")]
    public class UserDeletedEventHandler : DynamicIntegrationEventHandler
    {
        public override Task HandleDynamic(string eventName, dynamic eventData)
        {
            throw new NotImplementedException();
        }
    }
}
