using ApiClients.Abstractions.ChatApiClient.Participant.Requests;

namespace ApiClients.Abstractions.ChatApiClient.Participant;

public interface IChatParticipantApiClient
{
    Task<ApiResponse> UpdateParticipantRoleAsync(UpdateParticipantRoleRequest updateRoleRequest, CancellationToken cancellationToken = default);
}
