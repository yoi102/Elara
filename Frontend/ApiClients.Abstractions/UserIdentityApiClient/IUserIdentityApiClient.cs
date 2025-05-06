using ApiClients.Abstractions.UserIdentityApiClient.Responses;

namespace ApiClients.Abstractions.UserIdentityApiClient;

public interface IUserIdentityApiClient
{
    Task<ResetCodeResponse> GetResetCodeByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<LoginResponse> LoginByEmailAndPasswordAsync(string email, string password, string userAgent, CancellationToken cancellationToken = default);

    Task<LoginResponse> LoginByNameAndPasswordAsync(string name, string password, string userAgent, CancellationToken cancellationToken = default);

    Task<ApiResponse> ResetPasswordWithEmailCodeAsync(string email, string newPassword, string resetCode, CancellationToken cancellationToken = default);

    Task<ApiResponse> SignUpAsync(string name, string email, string password, CancellationToken cancellationToken = default);

    Task<RefreshTokenResponse> RefreshTokenAsync(Guid userId, string refreshToken, string userAgent, CancellationToken cancellationToken = default);
}
