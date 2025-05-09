﻿namespace Services.Abstractions.Results.Data;
public record MessageData
{
    public required Guid Id { get; init; }
    public required Guid ConversationId { get; init; }
    public required bool IsUnread { get; init; }
    public required QuoteMessageData? QuoteMessage { get; init; }
    public required string Content { get; init; }
    public required MessageSenderData Sender { get; init; }
    public required UploadedItemData[] UploadedItems { get; init; }
    public required ReplyMessageData[] ReplyMessages { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
    public required DateTimeOffset? UpdatedAt { get; init; }
}
