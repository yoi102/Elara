using ApiClients.Abstractions.FileApiClient.Responses;

namespace Services.Abstractions.Results.Data;

public record ReplyMessageData
{
    public required bool IsUnread { get; init; }
    public required Guid MessageId { get; init; }
    public required string Content { get; init; }
    public required QuoteMessageData? QuoteMessage { get; init; }
    public required UserInfoData Sender { get; init; }
    public required FileItemData[] FileItems { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
    public required DateTimeOffset? UpdatedAt { get; init; }
}
