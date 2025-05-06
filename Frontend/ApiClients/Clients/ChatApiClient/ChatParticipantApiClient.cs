using ApiClients.Abstractions;
using ApiClients.Abstractions.ChatApiClient.Participant;
using ApiClients.Abstractions.ChatApiClient.Participant.Requests;
using RestSharp;

namespace ApiClients.Clients.ChatApiClient;

public class ChatParticipantApiClient : IChatParticipantApiClient
{
    private const string serviceUri = "/ChatService/api/participant";
    private readonly ITokenRefreshingRestClient client;

    public ChatParticipantApiClient(ITokenRefreshingRestClient client)
    {
        this.client = client;
    }

    public async Task<ApiResponse> UpdateParticipantRoleAsync(UpdateParticipantRoleRequest updateRoleRequest, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest
        {
            Resource = serviceUri,
            Method = Method.Patch
        };
        request.AddBody(updateRoleRequest);
        var response = await client.ExecuteWithAutoRefreshAsync(request, cancellationToken);

        if (!response.IsSuccessful)
            return new ApiResponse { IsSuccessful = false, StatusCode = response.StatusCode, ErrorMessage = response.ErrorMessage };

        return new ApiResponse() { IsSuccessful = true, StatusCode = response.StatusCode };
    }
}
