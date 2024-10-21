using MediatR;

namespace SocialLink.Domain.Events.UserEvents
{

    public record class UserDeletedDomainEvent(Guid UserId) : INotification;

}
