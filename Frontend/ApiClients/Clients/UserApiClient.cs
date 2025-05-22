using ApiClients.Abstractions;
using ApiClients.Abstractions.Models;
using ApiClients.Abstractions.Models.Responses;
using Frontend.Shared.Exceptions;
using RestSharp;

namespace ApiClients.Clients;

internal class UserApiClient : IUserApiClient
{
    private const string serviceUri = "/IdentityService/api/users";
    private readonly ITokenRefreshingRestClient client;

    public UserApiClient(ITokenRefreshingRestClient client)
    {
        this.client = client;
    }

    public async Task<ApiResponse> DeleteAsync(CancellationToken cancellationToken = default)
    {
        var restRequest = new RestRequest
        {
            Resource = serviceUri,
            Method = Method.Delete
        };

        var restResponse = await client.ExecuteWithAutoRefreshAsync(restRequest, cancellationToken);

        if (!restResponse.IsSuccessful)
            return new ApiResponse { IsSuccessful = false, StatusCode = restResponse.StatusCode, ErrorMessage = restResponse.ErrorMessage };

        return new ApiResponse() { IsSuccessful = true, StatusCode = restResponse.StatusCode };
    }

    public async Task<ApiResponse<AccountInfoData>> GetUserInfoAsync(CancellationToken cancellationToken = default)
    {
        var restRequest = new RestRequest
        {
            Resource = serviceUri,
            Method = Method.Get
        };
        var restResponse = await client.ExecuteWithAutoRefreshAsync(restRequest, cancellationToken);

        if (!restResponse.IsSuccessful)
            return new ApiResponse<AccountInfoData> { IsSuccessful = false, StatusCode = restResponse.StatusCode, ErrorMessage = restResponse.ErrorMessage };

        if (string.IsNullOrEmpty(restResponse.Content))
            throw new ApiResponseException();

        var userInfo = JsonUtils.DeserializeInsensitive<AccountInfoData>(restResponse.Content);

        if (userInfo is null)
            throw new ApiResponseException();

        return new ApiResponse<AccountInfoData>() { IsSuccessful = true, StatusCode = restResponse.StatusCode, ResponseData = userInfo };
    }
}
