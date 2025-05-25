using ApiClients.Abstractions;
using Services.Abstractions.ChatServices;

namespace Services.ChatServices;

internal class ChatParticipantService : IChatParticipantService
{
    private readonly IChatParticipantApiClient chatParticipantApiClient;

    public ChatParticipantService(IChatParticipantApiClient chatParticipantApiClient)
    {
        this.chatParticipantApiClient = chatParticipantApiClient;
    }

    public async Task UpdateParticipantRoleAsync(Guid id, string role, CancellationToken cancellationToken = default)
    {
        var response = await chatParticipantApiClient.UpdateParticipantRoleAsync(id, role, cancellationToken);

        if (!response.IsSuccessful)
            throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);
    }
}
