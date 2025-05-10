using DomainCommons.EntityStronglyIds;

namespace ChatService.WebAPI.Controllers.Responses;

public record MessageResponse
{
    public required bool IsUnread { get; init; }
    public required MessageId MessageId { get; set; }
    public required ConversationId ConversationId { get; set; }
    public required MessageId? QuoteMessageId { get; set; }
    public required string Content { get; set; }
    public required UserId SenderId { get; set; }
    public required UploadedItemId[] UploadedItemIds { get; set; }
    public required DateTimeOffset CreatedAt { get; set; }
    public required DateTimeOffset? UpdatedAt { get; set; }
}
