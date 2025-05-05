using ApiClients.Abstractions.ChatApiClient.Participant;
using ApiClients.Abstractions.ChatApiClient.Participant.Requests;
using ApiClients.Abstractions.ChatApiClient.Participant.Responses;
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

    public async Task<UpdateParticipantRoleResponse> UpdateParticipantRoleAsync(UpdateParticipantRoleRequest updateRoleRequest, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest
        {
            Resource = serviceUri,
            Method = Method.Patch
        };
        request.AddBody(updateRoleRequest);
        var response = await client.ExecuteWithAutoRefreshAsync(request, cancellationToken);

        if (!response.IsSuccessful)
            return new UpdateParticipantRoleResponse { IsSuccessful = false, StatusCode = response.StatusCode, ErrorMessage = response.ErrorMessage };

        return new UpdateParticipantRoleResponse() { IsSuccessful = true, StatusCode = response.StatusCode };
    }
}
