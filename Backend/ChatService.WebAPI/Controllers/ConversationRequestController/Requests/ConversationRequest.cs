using DomainCommons.EntityStronglyIds;
using FluentValidation;

namespace ChatService.WebAPI.Controllers.ConversationRequestController.Requests;

public record ConversationRequestRequest(UserId ReceiverId, ConversationId ConversationId, string Role);

public class ConversationRequestRequestValidator : AbstractValidator<ConversationRequestRequest>
{
    public ConversationRequestRequestValidator()
    {
        RuleFor(e => e.ReceiverId.Value).NotEmpty();
        RuleFor(e => e.ConversationId.Value).NotEmpty();
        RuleFor(e => e.Role).NotNull().NotEmpty();
    }
}
