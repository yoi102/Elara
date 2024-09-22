using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace JWT;

public class TokenService : ITokenService
{
    private readonly JWTOptions _jwtOptions;

    public TokenService(IOptions<JWTOptions> jwtOptions)
    {
        this._jwtOptions = jwtOptions.Value;
    }

    public string BuildToken(IEnumerable<Claim> claims)
    {
        TimeSpan ExpiryDuration = TimeSpan.FromSeconds(_jwtOptions.ExpireSeconds);
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        var tokenDescriptor = new JwtSecurityToken(_jwtOptions.Issuer, _jwtOptions.Audience, claims,
            expires: DateTime.Now.Add(ExpiryDuration), signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }

    public string GenerateRefreshToken()
    {
        var randomBytes = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }
}
