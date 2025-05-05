using DomainCommons.EntityStronglyIds;
using FluentValidation;

namespace ChatService.WebAPI.Controllers.MessageController.Requests;

public record SendMessageRequest(ConversationId ConversationId, string Content, UploadedItemId[] MessageAttachmentIds, MessageId? QuoteMessage);

public class SendMessageRequestValidator : AbstractValidator<SendMessageRequest>
{
    public SendMessageRequestValidator()
    {
        RuleFor(e => e.ConversationId.Value).NotNull().NotEmpty();
        RuleFor(e => e.Content).NotNull();
        RuleFor(e => e.MessageAttachmentIds).NotNull();
    }
}
