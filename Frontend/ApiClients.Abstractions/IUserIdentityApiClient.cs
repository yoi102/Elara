using ApiClients.Abstractions.Models;
using ApiClients.Abstractions.Models.Responses;

namespace ApiClients.Abstractions;

public interface IUserIdentityApiClient
{
    Task<ApiResponse<ResetCodeData>> GetResetCodeByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<ApiResponse<UserTokenData>> LoginByEmailAndPasswordAsync(string email, string password, string userAgent, CancellationToken cancellationToken = default);

    Task<ApiResponse<UserTokenData>> LoginByNameAndPasswordAsync(string name, string password, string userAgent, CancellationToken cancellationToken = default);

    Task<ApiResponse> ResetPasswordWithEmailCodeAsync(string email, string newPassword, string resetCode, CancellationToken cancellationToken = default);

    Task<ApiResponse> SignUpAsync(string name, string email, string password, CancellationToken cancellationToken = default);

    Task<ApiResponse<UserTokenData>> RefreshTokenAsync(Guid userId, string refreshToken, string userAgent, CancellationToken cancellationToken = default);
}
