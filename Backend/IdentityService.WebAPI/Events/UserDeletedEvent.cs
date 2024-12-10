using DomainCommons.EntityStronglyIds;

namespace IdentityService.WebAPI.Events
{
    public record UserDeletedEvent(UserId Id);

}
