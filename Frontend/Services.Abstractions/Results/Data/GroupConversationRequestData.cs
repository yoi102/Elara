namespace Services.Abstractions.Results.Data;

public record GroupConversationRequestData
{
    public required Guid Id { get; init; }
    public required UserInfoData Sender { get; init; }
    public required string Role { get; init; }
    public required string ConversationName { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
    public required DateTimeOffset? UpdatedAt { get; init; }
}
