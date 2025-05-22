using DomainCommons.EntityStronglyIds;

namespace ChatService.WebAPI.Models.Responses;

public record ReplyMessageResponse
{
    public required MessageId MessageId { get; set; }
    public required ConversationId ConversationId { get; set; }
    public required UserInfoResponse Sender { get; set; }
    public required string Content { get; set; }
    public required DateTimeOffset SendAt { get; set; }
    public required DateTimeOffset? UpdatedAt { get; set; }
    public required bool IsUnread { get; set; }
    public required UploadedItemResponse[] Attachments { get; set; }
}
