using ApiClients.Abstractions;
using Frontend.Shared.Exceptions;
using Services.Abstractions;
using Services.Abstractions.ChatServices;

namespace Services.ChatServices;

internal class ChatParticipantService : IChatParticipantService
{
    private readonly IChatParticipantApiClient chatParticipantApiClient;

    public ChatParticipantService(IChatParticipantApiClient chatParticipantApiClient)
    {
        this.chatParticipantApiClient = chatParticipantApiClient;
    }

    public async Task<ApiServiceResult> UpdateParticipantRoleAsync(Guid id, string role, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await chatParticipantApiClient.UpdateParticipantRoleAsync(id, role, cancellationToken);

            if (!response.IsSuccessful)
                throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);
            return new ApiServiceResult()
            {
                IsSuccessful = true
            };
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode is null || (int)ex.StatusCode >= 500)
            {
                return new ApiServiceResult()
                {
                    IsSuccessful = false,
                    ErrorMessage = ex.Message,
                    IsServerError = true
                };
            }
            else if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new ApiServiceResult()
                {
                    IsSuccessful = false,
                    ErrorMessage = "Not Found",
                    IsServerError = true
                };
            }
            throw new ApiResponseException();
        }
    }
}
