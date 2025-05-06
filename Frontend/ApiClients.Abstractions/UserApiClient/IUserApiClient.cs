using ApiClients.Abstractions.UserIdentityApiClient.Responses;

namespace ApiClients.Abstractions.UserApiClient;

public interface IUserApiClient
{
    Task<ApiResponse> DeleteAsync(CancellationToken cancellationToken = default);

    Task<UserInfoResponse> GetUserInfoAsync(CancellationToken cancellationToken = default);

}
