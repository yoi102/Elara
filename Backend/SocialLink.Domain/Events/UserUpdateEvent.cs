using MediatR;
using SocialLink.Domain.Entities;

namespace SocialLink.Domain.Events
{
    public record class UserUpdateEvent(User User) : INotification
    {
    }
}
