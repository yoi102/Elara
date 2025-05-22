using ApiClients.Abstractions;
using ApiClients.Abstractions.Models;
using ApiClients.Abstractions.Models.Responses;
using Frontend.Shared.Exceptions;
using RestSharp;

namespace ApiClients.Clients.PersonalSpaceApiClient;

internal class PersonalSpaceContactApiClient : IPersonalSpaceContactApiClient
{
    private const string serviceUri = "/PersonalSpaceService/api/contacts";
    private readonly ITokenRefreshingRestClient client;

    public PersonalSpaceContactApiClient(ITokenRefreshingRestClient client)
    {
        this.client = client;
    }

    public async Task<ApiResponse> DeleteContactAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest
        {
            Resource = serviceUri + $"/{id}",
            Method = Method.Get
        };
        var response = await client.ExecuteWithAutoRefreshAsync(request, cancellationToken);

        if (!response.IsSuccessful)
            return new ApiResponse { IsSuccessful = false, StatusCode = response.StatusCode, ErrorMessage = response.ErrorMessage };

        return new ApiResponse() { IsSuccessful = true, StatusCode = response.StatusCode };
    }

    public async Task<ApiResponse<ContactData[]>> GetContactsAsync(CancellationToken cancellationToken = default)
    {
        var request = new RestRequest
        {
            Resource = serviceUri,
            Method = Method.Get
        };
        var response = await client.ExecuteWithAutoRefreshAsync(request, cancellationToken);

        if (!response.IsSuccessful)
            return new ApiResponse<ContactData[]> { IsSuccessful = false, StatusCode = response.StatusCode, ErrorMessage = response.ErrorMessage };

        if (string.IsNullOrEmpty(response.Content))
            throw new ApiResponseException();

        var data = JsonUtils.DeserializeInsensitive<ContactData[]>(response.Content);

        if (data is null)
            throw new ApiResponseException();

        return new ApiResponse<ContactData[]>() { IsSuccessful = true, StatusCode = response.StatusCode, ResponseData = data };
    }

    public async Task<ApiResponse> UpdateContactInfoAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest
        {
            Resource = serviceUri + $"/{id}",
            Method = Method.Get
        };
        var response = await client.ExecuteWithAutoRefreshAsync(request, cancellationToken);

        if (!response.IsSuccessful)
            return new ApiResponse { IsSuccessful = false, StatusCode = response.StatusCode, ErrorMessage = response.ErrorMessage };

        return new ApiResponse() { IsSuccessful = true, StatusCode = response.StatusCode };
    }
}
