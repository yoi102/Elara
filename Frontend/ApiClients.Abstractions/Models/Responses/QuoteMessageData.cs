namespace ApiClients.Abstractions.Models.Responses;
public record QuoteMessageData
{
    public required Guid MessageId { get; set; }
    public required Guid ConversationId { get; set; }
    public required UserInfoData Sender { get; set; }
    public required string Content { get; set; }
    public required DateTimeOffset SendAt { get; set; }
    public required DateTimeOffset? UpdatedAt { get; set; }
    public required UploadedItemData[] Attachments { get; set; }
}
