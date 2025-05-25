using ApiClients.Abstractions.Models.Responses;

namespace Services.Abstractions;

public interface IUserIdentityService
{
    Task SignUpAsync(string name, string email, string password, CancellationToken cancellationToken = default);

    Task<ResetCodeData> GetResetCodeByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<UserTokenData?> LoginByEmailAndPasswordAsync(string email, string password, CancellationToken cancellationToken = default);

    Task<UserTokenData?> LoginByNameAndPasswordAsync(string name, string password, CancellationToken cancellationToken = default);

    Task<UserTokenData> RefreshCurrentUserTokenAsync(CancellationToken cancellationToken = default);

    Task<UserTokenData> RefreshTokenAsync(Guid userId, string refreshToken, CancellationToken cancellationToken = default);

    Task ResetPasswordWithResetCodeAsync(string email, string newPassword, string resetCode, CancellationToken cancellationToken = default);
}
