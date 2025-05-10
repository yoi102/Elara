namespace ApiClients.Abstractions.ChatApiClient.Conversation.Responses;

public record MessagesResponse : ApiResponse<MessageData[]>;

public record MessageData()
{
    public required Guid Id { get; init; }
    public required Guid ConversationId { get; init; }
    public required Guid? QuoteMessageId { get; init; }
    public required string Content { get; init; }
    public required Guid SenderId { get; init; }
    public required Guid[] UploadedItemIds { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
    public required DateTimeOffset? UpdatedAt { get; init; }
}
