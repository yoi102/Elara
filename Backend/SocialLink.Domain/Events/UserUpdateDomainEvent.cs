using MediatR;
using SocialLink.Domain.Entities;

namespace SocialLink.Domain.Events
{
    public record class UserUpdateDomainEvent(User User) : INotification;

}
