using DomainCommons.EntityStronglyIds;

namespace IdentityService.Domain.Data;

public record ResetCodeCacheData(UserId UserId, string ResetCode);
