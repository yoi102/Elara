using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Domain.Interfaces
{
    public interface IResetTokenCacheService
    {
        string CacheToken(string token);
        string? FindTokenByResetCode(string resetCode);
    }
}
