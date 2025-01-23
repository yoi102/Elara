using DomainCommons.EntityStronglyIds;

namespace IdentityService.WebAPI.Events.Args;

public record UserCreatedEventArgs(UserId Id, string Name);