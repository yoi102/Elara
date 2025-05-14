using ApiClients.Abstractions.FileApiClient.Responses;

namespace Services.Abstractions.Results.Data;
public class QuoteMessageData
{
    public required Guid MessageId { get; init; }
    public required string Content { get; init; }
    public required UserInfoData Sender { get; init; }
    public required FileItemData[] FileItems { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
    public required DateTimeOffset? UpdatedAt { get; init; }
}
