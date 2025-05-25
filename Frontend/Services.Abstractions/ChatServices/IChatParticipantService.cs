namespace Services.Abstractions.ChatServices;

public interface IChatParticipantService
{
    Task UpdateParticipantRoleAsync(Guid id, string role, CancellationToken cancellationToken = default);
}
