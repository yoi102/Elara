using ApiClients.Abstractions.ChatApiClient.ConversationRequest;
using ApiClients.Abstractions.Models.Responses;
using Services.Abstractions.ChatServices;

namespace Services.ChatServices;

internal class ChatConversationRequestService : IChatConversationRequestService
{
    private readonly IChatConversationRequestApiClient chatConversationRequestApiClient;

    public ChatConversationRequestService(IChatConversationRequestApiClient chatConversationRequestApiClient)
    {
        this.chatConversationRequestApiClient = chatConversationRequestApiClient;
    }

    public async Task AcceptConversationRequestAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await chatConversationRequestApiClient.AcceptConversationRequestAsync(id, cancellationToken);

        if (!response.IsSuccessful)
            throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);
    }

    public async Task<ConversationRequestData?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await chatConversationRequestApiClient.FindByIdAsync(id, cancellationToken);

            if (!response.IsSuccessful)
                throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);

            return response.ResponseData;
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<ConversationRequestData[]> GetConversationRequestsAsync(CancellationToken cancellationToken = default)
    {
        var response = await chatConversationRequestApiClient.GetConversationRequestsAsync(cancellationToken);

        if (!response.IsSuccessful)
            throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);

        return response.ResponseData;
    }

    public async Task RejectConversationRequestAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await chatConversationRequestApiClient.RejectConversationRequestAsync(id, cancellationToken);

        if (!response.IsSuccessful)
            throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);
    }

    public async Task SendConversationRequestAsync(Guid receiverId, Guid conversationId, string role, CancellationToken cancellationToken = default)
    {
        var response = await chatConversationRequestApiClient.SendConversationRequestAsync(receiverId, conversationId, role, cancellationToken);

        if (!response.IsSuccessful)
            throw new HttpRequestException(response.ErrorMessage, null, response.StatusCode);
    }
}
