using DomainCommons.EntityStronglyIds;
using DomainCommons.ServiceInterface;
using IdentityService.Domain.Data;
using IdentityService.Domain.Interfaces;
using JWT;
using Microsoft.Extensions.Options;

namespace IdentityService.Infrastructure.Services;

public class CacheService : ICacheService
{
    private readonly JWTOptions jwtOptions;
    private readonly IRedisDbService redisDbService;

    //TODO：Radis应该用Set对应用户。
    public CacheService(IRedisDbService redisDbService,
                             IOptions<JWTOptions> jwtOptions)
    {
        this.redisDbService = redisDbService;
        this.jwtOptions = jwtOptions.Value;
    }

    public async Task<bool> CacheRefreshTokenCacheAsync(RefreshTokenData refreshTokenData)
    {
        return await redisDbService.SetAsync(refreshTokenData.RefreshToken, refreshTokenData, TimeSpan.FromDays(jwtOptions.RefreshTokenExpireDays));
    }

    public async Task<bool> CacheResetCodeCacheAsync(ResetCodeCacheData resetTokenData)
    {
        return await redisDbService.SetAsync(resetTokenData.UserId.ToString(), resetTokenData, TimeSpan.FromMinutes(jwtOptions.ResetTokenExpireMinutes));//ResetCode5分钟
    }

    public async Task<RefreshTokenData?> GetRefreshTokenCacheAsync(string refreshToken)
    {
        return await redisDbService.GetAsync<RefreshTokenData?>(refreshToken);
    }

    public async Task<ResetCodeCacheData?> GetResetCodeCacheAsync(UserId UserId)
    {
        return await redisDbService.GetAsync<ResetCodeCacheData?>(UserId.ToString());
    }

    public async Task<bool> RemoveAsync(string key)
    {
        return await redisDbService.RemoveAsync(key);
    }
}
