using DomainCommons.EntityStronglyIds;

namespace IdentityService.Domain.Data;

public record RefreshTokenData(UserId UserId, string RefreshToken, string UserAgent);

