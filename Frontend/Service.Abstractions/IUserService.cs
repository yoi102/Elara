using Service.Abstractions.UserResponses;

namespace Service.Abstractions
{
    public interface IUserService
    {
        Task<DeleteUserResponse> DeleteAsync(CancellationToken cancellationToken = default);

        Task<GetUserInfoResponse> GetUserInfoAsync(CancellationToken cancellationToken = default);

        Task<GetEmailResetCodeResponse> GetEmailResetCodeAsync(string email, CancellationToken cancellationToken = default);

        Task<LoginResponse> LoginByEmailAndPasswordAsync(string email, string password, CancellationToken cancellationToken = default);

        Task<LoginResponse> LoginByNameAndPasswordAsync(string name, string password, CancellationToken cancellationToken = default);

        Task<ResetPasswordResponse> ResetPasswordWithEmailCodeAsync(string email, string newPassword, string resetCode, CancellationToken cancellationToken = default);

        Task<CreateResponse> CreateAsync(string name, string email, string password, CancellationToken cancellationToken = default);
    }
}