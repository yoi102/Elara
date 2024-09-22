using Services.Abstractions.Results.Results;

namespace Services.Abstractions;

public interface IUserService
{
    Task<DeleteUserResult> DeleteAsync(CancellationToken cancellationToken = default);

    Task<UserInfoResult> GetUserInfoAsync(CancellationToken cancellationToken = default);
}
