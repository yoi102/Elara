namespace Services.Abstractions.Results.Results;

public record UnreadMessagesResult : ApiServiceResult<UnreadMessageResultData[]>;
public record UnreadMessageResultData(Guid Id, Guid ConversationId, Guid UserId, Guid MessageId);
