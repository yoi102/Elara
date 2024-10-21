using MediatR;
using SocialLink.Domain.Entities;

namespace SocialLink.Domain.Events.UserEvents
{
    public record class UserUpdateDomainEvent(User User) : INotification;

}
