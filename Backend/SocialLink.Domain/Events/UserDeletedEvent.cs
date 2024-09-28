using MediatR;

namespace SocialLink.Domain.Events
{

    public record class UserDeletedEvent(Guid UserId) : INotification
    {
    }
}
