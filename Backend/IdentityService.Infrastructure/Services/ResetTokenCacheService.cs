using IdentityService.Domain.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace IdentityService.Infrastructure.Services
{
    public class ResetTokenCacheService : IResetTokenCacheService
    {
        private readonly IMemoryCache memoryCache;

        public ResetTokenCacheService(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        public string CacheToken(string token)
        {
            var resetCode = UniqueResetCode();

            var entry = memoryCache.CreateEntry(resetCode);

            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15);
            entry.Value = token;
            return resetCode;
        }

        public string? FindTokenByResetCode(string resetCode)
        {
            if (!memoryCache.TryGetValue(resetCode, out var result))
            {
                return null;
            }
            memoryCache.Remove(resetCode);
            return result!.ToString();
        }

        private string UniqueResetCode()
        {
            var verificationCode = new Random().Next(100000, 999999).ToString();

            if (!memoryCache.TryGetValue(verificationCode, out var result))
            {
                UniqueResetCode();
            }
            return verificationCode;
        }
    }
}