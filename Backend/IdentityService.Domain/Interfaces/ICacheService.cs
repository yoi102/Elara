using DomainCommons.EntityStronglyIds;
using IdentityService.Domain.Data;

namespace IdentityService.Domain.Interfaces;

public interface ICacheService
{
    Task<bool> CacheRefreshTokenCacheAsync(RefreshTokenData refreshTokenData);

    Task<bool> CacheResetCodeCacheAsync(ResetCodeCacheData resetTokenData);

    Task<RefreshTokenData?> GetRefreshTokenCacheAsync(string refreshToken);

    Task<ResetCodeCacheData?> GetResetCodeCacheAsync(UserId UserId);

    Task<bool> RemoveAsync(string key);
}
