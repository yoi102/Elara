using RestSharp;

namespace ApiClients;

public interface ITokenRefreshingRestClient
{
    Task<RestResponse> ExecuteWithAutoRefreshAsync(RestRequest request, CancellationToken cancellationToken = default);
}
