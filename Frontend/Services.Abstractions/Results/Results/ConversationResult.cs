namespace Services.Abstractions.Results.Results;
public record ConversationResult : ApiServiceResult<ConversationResultData>;
public record ConversationsResult : ApiServiceResult<ConversationResultData[]>;

public record ConversationResultData(Guid Id, string Name, bool IsGroup, DateTimeOffset CreatedAt, DateTimeOffset? UpdatedAt);
