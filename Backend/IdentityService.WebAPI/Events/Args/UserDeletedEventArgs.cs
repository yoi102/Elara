using DomainCommons.EntityStronglyIds;

namespace IdentityService.WebAPI.Events.Args;

public record UserDeletedEventArgs(UserId Id, string userName);
