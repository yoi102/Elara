using ApiClients.Abstractions.ChatApiClient.ConversationRequest.Responses;
using Services.Abstractions.Results;

namespace Services.Abstractions.ChatServices;

public interface IChatConversationRequestService
{
    Task<ApiServiceResult> AcceptConversationRequestAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ApiServiceResult<ConversationRequestData>> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ApiServiceResult<ConversationRequestData[]>> GetConversationRequestsAsync(CancellationToken cancellationToken = default);

    Task<ApiServiceResult> RejectConversationRequestAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ApiServiceResult> SendConversationRequestAsync(Guid receiverId, Guid conversationId, string role, CancellationToken cancellationToken = default);
}
