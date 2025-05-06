using ApiClients.Abstractions;
using ApiClients.Abstractions.PersonalSpaceApiClient.Contact;
using ApiClients.Abstractions.PersonalSpaceApiClient.Contact.Responses;
using Frontend.Shared.Exceptions;
using RestSharp;

namespace ApiClients.Clients.PersonalSpaceApiClient;

public class PersonalSpaceContactApiClient : IPersonalSpaceContactApiClient
{
    private const string serviceUri = "/PersonalSpaceService/api/contacts";
    private readonly ITokenRefreshingRestClient client;

    public PersonalSpaceContactApiClient(ITokenRefreshingRestClient client)
    {
        this.client = client;
    }

    public async Task<ApiResponse> DeleteContactAsync(Guid contactId, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest
        {
            Resource = serviceUri + $"/{contactId}",
            Method = Method.Get
        };
        var response = await client.ExecuteWithAutoRefreshAsync(request, cancellationToken);

        if (!response.IsSuccessful)
            return new ApiResponse { IsSuccessful = false, StatusCode = response.StatusCode, ErrorMessage = response.ErrorMessage };


        return new ApiResponse() { IsSuccessful = true, StatusCode = response.StatusCode };
    }

    public async Task<GetAllContactsResponse> GetContactsAsync(CancellationToken cancellationToken = default)
    {
        var request = new RestRequest
        {
            Resource = serviceUri,
            Method = Method.Get
        };
        var response = await client.ExecuteWithAutoRefreshAsync(request, cancellationToken);

        if (!response.IsSuccessful)
            return new GetAllContactsResponse { IsSuccessful = false, StatusCode = response.StatusCode, ErrorMessage = response.ErrorMessage };

        if (string.IsNullOrEmpty(response.Content))
            throw new ApiResponseException();

        var data = JsonUtils.DeserializeInsensitive<ContactData[]>(response.Content);

        if (data is null)
            throw new ApiResponseException();

        return new GetAllContactsResponse() { IsSuccessful = true, StatusCode = response.StatusCode, ResponseData = data };
    }

    public async Task<ApiResponse> UpdateContactInfoAsync(Guid contactId, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest
        {
            Resource = serviceUri + $"/{contactId}",
            Method = Method.Get
        };
        var response = await client.ExecuteWithAutoRefreshAsync(request, cancellationToken);

        if (!response.IsSuccessful)
            return new ApiResponse { IsSuccessful = false, StatusCode = response.StatusCode, ErrorMessage = response.ErrorMessage };


        return new ApiResponse() { IsSuccessful = true, StatusCode = response.StatusCode };
    }
}
