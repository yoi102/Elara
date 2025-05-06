using Frontend.Shared.Exceptions;
using RestSharp;
using Services.Abstractions;
using System.Net;

namespace ApiClients;

internal class TokenRefreshingRestClient : ITokenRefreshingRestClient
{
    private readonly IRestClient client;
    private readonly IUserIdentityService userIdentityService;

    public TokenRefreshingRestClient(IRestClient client, IUserIdentityService userIdentityService)
    {
        this.client = client;
        this.userIdentityService = userIdentityService;
    }

    public async Task<RestResponse> ExecuteWithAutoRefreshAsync(RestRequest request, CancellationToken cancellationToken = default)
    {
        var response = await client.ExecuteAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            var accessToken = await userIdentityService.RefreshCurrentUserTokenAsync(cancellationToken);

            if (!accessToken.IsSuccessful)
                throw new ForceLogoutException();

            client.AddDefaultHeader("Authorization", $"Bearer {accessToken.ResultData.AccessToken}");

            response = await client.ExecuteAsync(request, cancellationToken);
        }

        return response;
    }
}
