using MediatR;

namespace SocialLink.Domain.Events
{

    public record class UserDeletedDomainEvent(Guid UserId) : INotification;

}
