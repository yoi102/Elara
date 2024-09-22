using ApiClients.Abstractions.UserApiClient.Responses;
using ApiClients.Abstractions.UserIdentityApiClient.Responses;

namespace ApiClients.Abstractions.UserApiClient;

public interface IUserApiClient
{
    Task<DeleteUserResponse> DeleteAsync(CancellationToken cancellationToken = default);

    Task<UserInfoResponse> GetUserInfoAsync(CancellationToken cancellationToken = default);

}
