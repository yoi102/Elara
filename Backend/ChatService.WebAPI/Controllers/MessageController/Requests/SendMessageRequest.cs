using DomainCommons.EntityStronglyIds;
using FluentValidation;

namespace ChatService.WebAPI.Controllers.MessageController.Requests;

public record SendMessageRequest(ConversationId ConversationId, string Content, UploadedItemId[] UploadedItemIds, MessageId? QuoteMessage);

public class SendMessageRequestValidator : AbstractValidator<SendMessageRequest>
{
    public SendMessageRequestValidator()
    {
        RuleFor(e => e.ConversationId.Value).NotNull().NotEmpty();
        RuleFor(e => e.Content).NotNull();
        RuleFor(e => e.UploadedItemIds).NotNull();
    }
}
