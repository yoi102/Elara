﻿using ApiClients.Abstractions;
using ApiClients.Abstractions.Models;
using ApiClients.Abstractions.Models.Responses;
using Frontend.Shared.Exceptions;
using RestSharp;

namespace ApiClients.Clients.PersonalSpaceApiClient;

internal class PersonalSpaceContactRequestApiClient : IPersonalSpaceContactRequestApiClient
{
    private const string serviceUri = "/PersonalSpaceService/api/contact-requests";
    private readonly ITokenRefreshingRestClient client;

    public PersonalSpaceContactRequestApiClient(ITokenRefreshingRestClient client)
    {
        this.client = client;
    }

    public async Task<ApiResponse> AcceptContactRequestAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest
        {
            Resource = serviceUri + $"/{id}/accept",
            Method = Method.Patch
        };
        var response = await client.ExecuteWithAutoRefreshAsync(request, cancellationToken);

        if (!response.IsSuccessful)
            return new ApiResponse { IsSuccessful = false, StatusCode = response.StatusCode, ErrorMessage = response.ErrorMessage };

        return new ApiResponse() { IsSuccessful = true, StatusCode = response.StatusCode };
    }

    public async Task<ApiResponse<ContactRequestData>> GetContactRequestByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest
        {
            Resource = serviceUri + $"{id}",
            Method = Method.Get
        };
        var response = await client.ExecuteWithAutoRefreshAsync(request, cancellationToken);

        if (!response.IsSuccessful)
            return new ApiResponse<ContactRequestData> { IsSuccessful = false, StatusCode = response.StatusCode, ErrorMessage = response.ErrorMessage };

        if (string.IsNullOrEmpty(response.Content))
            throw new ApiResponseException();

        var data = JsonUtils.DeserializeInsensitive<ContactRequestData>(response.Content);

        if (data is null)
            throw new ApiResponseException();

        return new ApiResponse<ContactRequestData>() { IsSuccessful = true, StatusCode = response.StatusCode, ResponseData = data };
    }

    public async Task<ApiResponse<ContactRequestData[]>> GetReceivedContactRequestsAsync(CancellationToken cancellationToken = default)
    {
        var request = new RestRequest
        {
            Resource = serviceUri,
            Method = Method.Get
        };
        var response = await client.ExecuteWithAutoRefreshAsync(request, cancellationToken);

        if (!response.IsSuccessful)
            return new ApiResponse<ContactRequestData[]> { IsSuccessful = false, StatusCode = response.StatusCode, ErrorMessage = response.ErrorMessage };

        if (string.IsNullOrEmpty(response.Content))
            throw new ApiResponseException();

        var data = JsonUtils.DeserializeInsensitive<ContactRequestData[]>(response.Content);

        if (data is null)
            throw new ApiResponseException();

        return new ApiResponse<ContactRequestData[]>() { IsSuccessful = true, StatusCode = response.StatusCode, ResponseData = data };
    }

    public async Task<ApiResponse> RejectContactRequestAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest
        {
            Resource = serviceUri + $"/{id}/reject",
            Method = Method.Patch
        };
        var response = await client.ExecuteWithAutoRefreshAsync(request, cancellationToken);

        if (!response.IsSuccessful)
            return new ApiResponse { IsSuccessful = false, StatusCode = response.StatusCode, ErrorMessage = response.ErrorMessage };

        return new ApiResponse() { IsSuccessful = true, StatusCode = response.StatusCode };
    }

    public async Task<ApiResponse> SendContactRequestAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest
        {
            Resource = serviceUri + $"/{id}",
            Method = Method.Post
        };
        var response = await client.ExecuteWithAutoRefreshAsync(request, cancellationToken);

        if (!response.IsSuccessful)
            return new ApiResponse { IsSuccessful = false, StatusCode = response.StatusCode, ErrorMessage = response.ErrorMessage };

        return new ApiResponse() { IsSuccessful = true, StatusCode = response.StatusCode };
    }
}
