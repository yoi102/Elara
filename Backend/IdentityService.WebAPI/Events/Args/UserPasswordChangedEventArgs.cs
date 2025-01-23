using DomainCommons.EntityStronglyIds;

namespace IdentityService.WebAPI.Events.Args;

public record class UserPasswordChangedEventArgs(UserId Id, string userName);
