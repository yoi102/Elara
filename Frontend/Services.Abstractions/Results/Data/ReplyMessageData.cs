﻿namespace Services.Abstractions.Results.Data;

public record ReplyMessageData
{
    public required bool IsUnread { get; init; }
    public required Guid MessageId { get; init; }
    public required string Content { get; init; }
    public required QuoteMessageData? QuoteMessage { get; init; }
    public required MessageSenderData Sender { get; init; }
    public required UploadedItemData[] UploadedItems { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
    public required DateTimeOffset? UpdatedAt { get; init; }
}
