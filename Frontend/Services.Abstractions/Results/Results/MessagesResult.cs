namespace Services.Abstractions.Results.Results;

public record MessagesResult : ApiServiceResult<MessageResultData[]>;
public record MessageResult : ApiServiceResult<MessageResultData>;
public record MessageResultData(Guid MessageId, Guid ConversationId, Guid? QuoteMessageId, string Content, Guid SenderId, Guid[] UploadedItemIds, DateTimeOffset CreatedAt, DateTimeOffset? UpdatedAt);
