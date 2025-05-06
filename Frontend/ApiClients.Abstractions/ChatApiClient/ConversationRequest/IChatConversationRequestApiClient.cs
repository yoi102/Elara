using ApiClients.Abstractions.ChatApiClient.ConversationRequest.Responses;

namespace ApiClients.Abstractions.ChatApiClient.ConversationRequest;

public interface IChatConversationRequestApiClient
{
    Task<ApiResponse> AcceptConversationRequestAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ConversationRequestResponse> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ConversationRequestsResponse> GetConversationRequestsAsync(CancellationToken cancellationToken = default);

    Task<ApiResponse> RejectConversationRequestAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ApiResponse> SendConversationRequestAsync(Guid receiverId, Guid conversationId, string role, CancellationToken cancellationToken = default);
}
