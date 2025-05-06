using Services.Abstractions.Results;
using Services.Abstractions.Results.Results;

namespace Services.Abstractions;

public interface IUserIdentityService
{
    Task<ApiServiceResult> SignUpAsync(string name, string email, string password, CancellationToken cancellationToken = default);

    Task<ResetCodeResult> GetResetCodeByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<ApiServiceResult> LoginByEmailAndPasswordAsync(string email, string password, CancellationToken cancellationToken = default);

    Task<ApiServiceResult> LoginByNameAndPasswordAsync(string name, string password, CancellationToken cancellationToken = default);

    Task<TokenReflashResult> RefreshCurrentUserTokenAsync(CancellationToken cancellationToken = default);

    Task<TokenReflashResult> RefreshTokenAsync(Guid userId, string refreshToken, CancellationToken cancellationToken = default);

    Task<ApiServiceResult> ResetPasswordWithResetCodeAsync(string email, string newPassword, string resetCode, CancellationToken cancellationToken = default);
}
