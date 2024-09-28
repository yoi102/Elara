using SocialLink.Domain.Entities;

namespace SocialLink.WebAPI.Events
{
    public record UserCreatedEvent(UserId Id, string Name, string Password);

}
