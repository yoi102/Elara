using DomainCommons.EntityStronglyIds;

namespace ChatService.WebAPI.Controllers.MessageController.Responses;

public record MessageResponse
{
    public MessageId Id { get; set; }
    public ConversationId ConversationId { get; set; }
    public MessageId? QuoteMessages { get; set; }
    public required string Content { get; set; }
    public UserId SenderId { get; set; }
    public required UploadedItemId[] UploadedItemIds { get; set; }
}
