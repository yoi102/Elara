using ApiClients.Abstractions.ChatApiClient.ConversationRequest.Responses;

namespace ApiClients.Abstractions.ChatApiClient.ConversationRequest;

public interface IChatConversationRequestApiClient
{
    Task<AcceptConversationRequestResponse> AcceptConversationRequestAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ConversationRequestResponse> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ConversationRequestsResponse> GetConversationRequestsAsync(CancellationToken cancellationToken = default);

    Task<RejectConversationRequestResponse> RejectConversationRequestAsync(Guid id, CancellationToken cancellationToken = default);

    Task<SendConversationRequestResponse> SendConversationRequestAsync(Guid receiverId, Guid conversationId, string role, CancellationToken cancellationToken = default);
}
