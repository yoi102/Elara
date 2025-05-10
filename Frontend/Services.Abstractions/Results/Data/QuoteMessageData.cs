namespace Services.Abstractions.Results.Data;
public class QuoteMessageData
{
    public required Guid MessageId { get; init; }
    public required string Content { get; init; }
    public required MessageSenderData Sender { get; init; }
    public required UploadedItemData[] UploadedItems { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
    public required DateTimeOffset? UpdatedAt { get; init; }
}
