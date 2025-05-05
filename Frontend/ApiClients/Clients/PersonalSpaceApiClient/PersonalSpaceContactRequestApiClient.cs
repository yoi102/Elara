using ApiClients.Abstractions.PersonalSpaceApiClient.ContactRequest;
using ApiClients.Abstractions.PersonalSpaceApiClient.ContactRequest.Responses;
using Frontend.Shared.Exceptions;
using RestSharp;

namespace ApiClients.Clients.PersonalSpaceApiClient;

public class PersonalSpaceContactRequestApiClient : IPersonalSpaceContactRequestApiClient
{
    private const string serviceUri = "/PersonalSpaceService/api/contact-requests";
    private readonly ITokenRefreshingRestClient client;

    public PersonalSpaceContactRequestApiClient(ITokenRefreshingRestClient client)
    {
        this.client = client;
    }

    public async Task<AcceptContactRequestResponse> AcceptContactRequestAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest
        {
            Resource = serviceUri + $"/{id}/accept",
            Method = Method.Patch
        };
        var response = await client.ExecuteWithAutoRefreshAsync(request, cancellationToken);

        if (!response.IsSuccessful)
            return new AcceptContactRequestResponse { IsSuccessful = false, StatusCode = response.StatusCode, ErrorMessage = response.ErrorMessage };

        return new AcceptContactRequestResponse() { IsSuccessful = true, StatusCode = response.StatusCode };
    }

    public async Task<GetContactRequestsResponse> GetReceivedContactRequestsAsync(CancellationToken cancellationToken = default)
    {
        var request = new RestRequest
        {
            Resource = serviceUri,
            Method = Method.Get
        };
        var response = await client.ExecuteWithAutoRefreshAsync(request, cancellationToken);

        if (!response.IsSuccessful)
            return new GetContactRequestsResponse { IsSuccessful = false, StatusCode = response.StatusCode, ErrorMessage = response.ErrorMessage };

        if (string.IsNullOrEmpty(response.Content))
            throw new ApiResponseException();

        var data = JsonUtils.DeserializeInsensitive<ContactRequestData[]>(response.Content);

        if (data is null)
            throw new ApiResponseException();

        return new GetContactRequestsResponse() { IsSuccessful = true, StatusCode = response.StatusCode, ResponseData = data };
    }

    public async Task<RejectContactRequestResponse> RejectContactRequestAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest
        {
            Resource = serviceUri + $"/{id}/reject",
            Method = Method.Patch
        };
        var response = await client.ExecuteWithAutoRefreshAsync(request, cancellationToken);

        if (!response.IsSuccessful)
            return new RejectContactRequestResponse { IsSuccessful = false, StatusCode = response.StatusCode, ErrorMessage = response.ErrorMessage };

        return new RejectContactRequestResponse() { IsSuccessful = true, StatusCode = response.StatusCode };
    }

    public async Task<SendContactRequestResponse> SendContactRequestAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest
        {
            Resource = serviceUri + $"/{id}",
            Method = Method.Post
        };
        var response = await client.ExecuteWithAutoRefreshAsync(request, cancellationToken);

        if (!response.IsSuccessful)
            return new SendContactRequestResponse { IsSuccessful = false, StatusCode = response.StatusCode, ErrorMessage = response.ErrorMessage };


        return new SendContactRequestResponse() { IsSuccessful = true, StatusCode = response.StatusCode };
    }
}
