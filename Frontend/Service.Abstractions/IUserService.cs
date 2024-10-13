namespace Service.Abstractions
{
    public interface IUserService
    {
        Task<bool> DeleteAsync(CancellationToken cancellationToken = default);

        Task<string?> GetUserInfoAsync(CancellationToken cancellationToken = default);

        Task<bool> GetEmailResetCodeAsync(string email, CancellationToken cancellationToken = default);

        Task<bool> LoginByEmailAndPasswordAsync(string email, string password, CancellationToken cancellationToken = default);

        Task<bool> LoginByNameAndPasswordAsync(string name, string password, CancellationToken cancellationToken = default);

        Task<bool> ResetPasswordWithEmailCodeAsync(string email, string newPassword, string resetCode, CancellationToken cancellationToken = default);

        Task<bool> SignUpAsync(string name, string email, string password, CancellationToken cancellationToken = default);
    }
}