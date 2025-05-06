namespace ApiClients.Abstractions.ChatApiClient.Participant;

public interface IChatParticipantApiClient
{
    Task<ApiResponse> UpdateParticipantRoleAsync(Guid id, string role, CancellationToken cancellationToken = default);
}
