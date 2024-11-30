using DomainCommons.EntityStronglyIds;

namespace IdentityService.WebAPI.Events
{
    public record UserCreatedEvent(UserId Id, string Name, string Password);
}