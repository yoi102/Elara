namespace Services.Abstractions.Results.Results;

public record ConversationRequestsResult : ApiServiceResult<ConversationRequestResultData[]>;
public record ConversationRequestResult : ApiServiceResult<ConversationRequestResultData>;
public record ConversationRequestResultData(Guid SenderId, Guid ReceiverId, Guid ConversationId, string Role, DateTimeOffset CreatedAt, DateTimeOffset? UpdatedAt);
