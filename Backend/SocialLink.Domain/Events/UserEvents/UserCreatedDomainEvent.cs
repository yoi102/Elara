using MediatR;
using SocialLink.Domain.Entities;

namespace SocialLink.Domain.Events.UserEvents
{

    public record class UserCreatedDomainEvent(User User) : INotification;

}
