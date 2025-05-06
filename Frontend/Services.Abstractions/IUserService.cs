using Services.Abstractions.Results;
using Services.Abstractions.Results.Results;

namespace Services.Abstractions;

public interface IUserService
{
    Task<ApiServiceResult> DeleteAsync(CancellationToken cancellationToken = default);

    Task<UserInfoResult> GetUserInfoAsync(CancellationToken cancellationToken = default);
}
