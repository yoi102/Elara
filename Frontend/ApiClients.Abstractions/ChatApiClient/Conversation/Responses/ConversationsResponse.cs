namespace ApiClients.Abstractions.ChatApiClient.Conversation.Responses;

public record ConversationsResponse : ApiResponse<ConversationData[]>;
public record ConversationResponse : ApiResponse<ConversationData>;

public record ConversationData(Guid Id, string Name, bool IsGroup, DateTimeOffset CreatedAt, DateTimeOffset? UpdatedAt);
