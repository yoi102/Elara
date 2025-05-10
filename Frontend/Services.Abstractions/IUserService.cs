using ApiClients.Abstractions.UserIdentityApiClient.Responses;
using Services.Abstractions.Results;

namespace Services.Abstractions;

public interface IUserService
{
    Task<ApiServiceResult> DeleteAsync(CancellationToken cancellationToken = default);

    Task<ApiServiceResult<UserInfo>> GetUserInfoAsync(CancellationToken cancellationToken = default);
}
