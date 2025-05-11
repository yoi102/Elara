namespace ApiClients.Abstractions.ChatApiClient.Conversation.Responses;

public record ParticipantsResponse : ApiResponse<ParticipantData[]>;

public record ParticipantData(Guid Id, Guid UserId, Guid ConversationId, string Role, bool IsGroup, DateTimeOffset CreatedAt, DateTimeOffset? UpdatedAt);
