using IdentityService.Domain.Entities;

namespace IdentityService.WebAPI.Events
{
    public record UserCreatedEvent(UserId Id, string Name, string Password);
}