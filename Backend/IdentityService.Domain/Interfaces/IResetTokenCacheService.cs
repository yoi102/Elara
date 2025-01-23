namespace IdentityService.Domain.Interfaces;

public interface IResetTokenCacheService
{
    string CacheToken(string token);
    string? FindTokenByResetCode(string resetCode);
}
