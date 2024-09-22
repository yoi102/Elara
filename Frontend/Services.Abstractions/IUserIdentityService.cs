using Services.Abstractions.Results.Results;

namespace Services.Abstractions;

public interface IUserIdentityService
{
    Task<CreateResult> SignUpAsync(string name, string email, string password, CancellationToken cancellationToken = default);

    Task<ResetCodeResult> GetResetCodeByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<LoginResult> LoginByEmailAndPasswordAsync(string email, string password, CancellationToken cancellationToken = default);

    Task<LoginResult> LoginByNameAndPasswordAsync(string name, string password, CancellationToken cancellationToken = default);

    Task<TokenReflashResult> RefreshCurrentUserTokenAsync(CancellationToken cancellationToken = default);

    Task<TokenReflashResult> RefreshTokenAsync(Guid userId, string refreshToken, CancellationToken cancellationToken = default);

    Task<ResetPasswordResult> ResetPasswordWithResetCodeAsync(string email, string newPassword, string resetCode, CancellationToken cancellationToken = default);
}
