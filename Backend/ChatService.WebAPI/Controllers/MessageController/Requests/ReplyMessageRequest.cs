using DomainCommons.EntityStronglyIds;
using FluentValidation;

namespace ChatService.WebAPI.Controllers.MessageController.Requests;

public record ReplyMessageRequest(MessageId MessageId, ConversationId ConversationId, string Content, UploadedItemId[] MessageAttachmentIds, MessageId? QuoteMessage);

public class ReplyMessageRequestValidator : AbstractValidator<ReplyMessageRequest>
{
    public ReplyMessageRequestValidator()
    {
        RuleFor(e => e.MessageId.Value).NotNull().NotEmpty();
        RuleFor(e => e.ConversationId.Value).NotNull().NotEmpty();
        RuleFor(e => e.Content).NotNull();
        RuleFor(e => e.MessageAttachmentIds).NotNull();
    }
}
