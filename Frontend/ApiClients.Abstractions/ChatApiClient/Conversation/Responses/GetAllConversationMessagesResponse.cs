namespace ApiClients.Abstractions.ChatApiClient.Conversation.Responses;

public record GetAllConversationMessagesResponse : ApiResponse<ConversationMessageData[]>;

public record ConversationMessageData()
{
    public required Guid MessageId { get; set; }
    public required Guid ConversationId { get; set; }
    public required Guid? QuoteMessageId { get; set; }
    public required string Content { get; set; }
    public required Guid SenderId { get; set; }
    public required Guid[] UploadedItemIds { get; set; }
    public required DateTimeOffset CreatedAt { get; set; }
    public required DateTimeOffset? UpdatedAt { get; set; }
}
