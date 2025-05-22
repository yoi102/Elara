using ApiClients.Abstractions.Models;

namespace ApiClients.Abstractions;

public interface IChatParticipantApiClient
{
    Task<ApiResponse> UpdateParticipantRoleAsync(Guid id, string role, CancellationToken cancellationToken = default);
}
