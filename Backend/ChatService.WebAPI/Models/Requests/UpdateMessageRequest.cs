using DomainCommons.EntityStronglyIds;
using FluentValidation;

namespace ChatService.WebAPI.Models.Requests;

public record UpdateMessageRequest(MessageId MessageId, string Content, UploadedItemId[] MessageAttachmentIds, MessageId? QuoteMessage);

public class UpdateMessageRequestValidator : AbstractValidator<UpdateMessageRequest>
{
    public UpdateMessageRequestValidator()
    {
        RuleFor(e => e.MessageId.Value).NotNull().NotEmpty();
        RuleFor(e => e.Content).NotNull();
        RuleFor(e => e.MessageAttachmentIds).NotNull();
    }
}
