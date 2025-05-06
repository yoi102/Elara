using Services.Abstractions.Results;
using Services.Abstractions.Results.Results;

namespace Services.Abstractions.ChatServices;

public interface IChatConversationService
{
    Task<ApiServiceResult> AcceptConversationRequestAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ConversationRequestResult> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ConversationRequestsResult> GetConversationRequestsAsync(CancellationToken cancellationToken = default);

    Task<ApiServiceResult> RejectConversationRequestAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ApiServiceResult> SendConversationRequestAsync(Guid receiverId, Guid conversationId, string role, CancellationToken cancellationToken = default);
}
