namespace ApiClients.Abstractions.ChatApiClient.Conversation.Responses;

public record UnreadMessagesResponse : ApiResponse<UnreadMessageData[]>;
public record UnreadMessageData(Guid Id, Guid ConversationId, Guid UserId, Guid MessageId);
