using ApiClients.Abstractions.ChatApiClient.Participant.Requests;
using ApiClients.Abstractions.ChatApiClient.Participant.Responses;

namespace ApiClients.Abstractions.ChatApiClient.Participant;

public interface IChatParticipantApiClient
{
    Task<UpdateParticipantRoleResponse> UpdateParticipantRoleAsync(UpdateParticipantRoleRequest updateRoleRequest, CancellationToken cancellationToken = default);
}
