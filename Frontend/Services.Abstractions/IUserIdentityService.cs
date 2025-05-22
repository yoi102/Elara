using ApiClients.Abstractions.Models.Responses;

namespace Services.Abstractions;

public interface IUserIdentityService
{
    Task<ApiServiceResult> SignUpAsync(string name, string email, string password, CancellationToken cancellationToken = default);

    Task<ApiServiceResult<ResetCodeData>> GetResetCodeByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<ApiServiceResult<UserTokenData>> LoginByEmailAndPasswordAsync(string email, string password, CancellationToken cancellationToken = default);

    Task<ApiServiceResult<UserTokenData>> LoginByNameAndPasswordAsync(string name, string password, CancellationToken cancellationToken = default);

    Task<ApiServiceResult<UserTokenData>> RefreshCurrentUserTokenAsync(CancellationToken cancellationToken = default);

    Task<ApiServiceResult<UserTokenData>> RefreshTokenAsync(Guid userId, string refreshToken, CancellationToken cancellationToken = default);

    Task<ApiServiceResult> ResetPasswordWithResetCodeAsync(string email, string newPassword, string resetCode, CancellationToken cancellationToken = default);
}
