using ApiClients.Abstractions.UserApiClient;
using ApiClients.Abstractions.UserApiClient.Responses;
using ApiClients.Abstractions.UserIdentityApiClient.Responses;
using Frontend.Shared.Exceptions;
using RestSharp;
using System.Text.Json;

namespace ApiClients.Clients;

public class UserApiClient : IUserApiClient
{
    private const string serviceUri = "/IdentityService/api/users";
    private readonly ITokenRefreshingRestClient client;

    public UserApiClient(ITokenRefreshingRestClient client)
    {
        this.client = client;
    }

    public async Task<DeleteUserResponse> DeleteAsync(CancellationToken cancellationToken = default)
    {
        var restRequest = new RestRequest
        {
            Resource = serviceUri,
            Method = Method.Delete
        };

        var restResponse = await client.ExecuteWithAutoRefreshAsync(restRequest, cancellationToken);

        if (!restResponse.IsSuccessful)
            return new DeleteUserResponse { IsSuccessful = false, StatusCode = restResponse.StatusCode, ErrorMessage = restResponse.ErrorMessage };

        return new DeleteUserResponse() { IsSuccessful = true, StatusCode = restResponse.StatusCode };
    }

    public async Task<UserInfoResponse> GetUserInfoAsync(CancellationToken cancellationToken = default)
    {
        var restRequest = new RestRequest
        {
            Resource = serviceUri,
            Method = Method.Get
        };
        var restResponse = await client.ExecuteWithAutoRefreshAsync(restRequest, cancellationToken);

        if (!restResponse.IsSuccessful)
            return new UserInfoResponse { IsSuccessful = false, StatusCode = restResponse.StatusCode, ErrorMessage = restResponse.ErrorMessage };

        if (string.IsNullOrEmpty(restResponse.Content))
            throw new ApiResponseException();

        var userInfo = JsonUtils.DeserializeInsensitive<UserInfo>(restResponse.Content);

        if (userInfo is null)
            throw new ApiResponseException();

        return new UserInfoResponse() { IsSuccessful = true, StatusCode = restResponse.StatusCode, ResponseData = userInfo };
    }
}
