using EventBus.SignalR.Client;

namespace Elara.ViewModel.SignalRHandlers;

[EventName("XXXXXEvent")]
internal class XXXXEventHandler : IEventHandler
{
    public Task HandleAsync()
    {
        throw new NotImplementedException();
    }
}
