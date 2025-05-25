using ApiClients.Abstractions.Models.Responses;

namespace Services.Abstractions.ChatServices;

public interface IChatConversationRequestService
{
    Task AcceptConversationRequestAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ConversationRequestData?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ConversationRequestData[]> GetConversationRequestsAsync(CancellationToken cancellationToken = default);

    Task RejectConversationRequestAsync(Guid id, CancellationToken cancellationToken = default);

    Task SendConversationRequestAsync(Guid receiverId, Guid conversationId, string role, CancellationToken cancellationToken = default);
}
