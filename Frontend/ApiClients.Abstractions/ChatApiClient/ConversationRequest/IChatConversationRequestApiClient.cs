using ApiClients.Abstractions.Models;
using ApiClients.Abstractions.Models.Responses;

namespace ApiClients.Abstractions.ChatApiClient.ConversationRequest;

public interface IChatConversationRequestApiClient
{
    Task<ApiResponse> AcceptConversationRequestAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ApiResponse<ConversationRequestData>> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ApiResponse<ConversationRequestData[]>> GetConversationRequestsAsync(CancellationToken cancellationToken = default);

    Task<ApiResponse> RejectConversationRequestAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ApiResponse> SendConversationRequestAsync(Guid receiverId, Guid conversationId, string role, CancellationToken cancellationToken = default);
}
