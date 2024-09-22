using System.Security.Claims;

namespace JWT;

public interface ITokenService
{
    string BuildToken(IEnumerable<Claim> claims);

    string GenerateRefreshToken();
}
