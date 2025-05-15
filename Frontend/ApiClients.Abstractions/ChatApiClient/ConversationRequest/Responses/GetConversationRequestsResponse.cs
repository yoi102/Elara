namespace ApiClients.Abstractions.ChatApiClient.ConversationRequest.Responses;

public record ConversationRequestsResponse : ApiResponse<ConversationRequestData[]>;
public record ConversationRequestResponse : ApiResponse<ConversationRequestData>;
public record ConversationRequestData(Guid Id, Guid SenderId, Guid ReceiverId, Guid ConversationId, string Role, DateTimeOffset CreatedAt, DateTimeOffset? UpdatedAt);
