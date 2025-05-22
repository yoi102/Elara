namespace Services.Abstractions.ChatServices;

public interface IChatParticipantService
{
    Task<ApiServiceResult> UpdateParticipantRoleAsync(Guid id, string role, CancellationToken cancellationToken = default);
}
