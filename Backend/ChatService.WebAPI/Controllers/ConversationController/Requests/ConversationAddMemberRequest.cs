using DomainCommons.EntityStronglyIds;
using FluentValidation;

namespace ChatService.WebAPI.Controllers.ConversationController.Requests;

public record ConversationAddMemberRequest(UserId UserId, string Role);

public class ConversationAddMemberRequestValidator : AbstractValidator<ConversationAddMemberRequest>
{
    public ConversationAddMemberRequestValidator()
    {
        RuleFor(e => e.UserId.Value).NotEmpty();
        RuleFor(e => e.Role).NotNull().NotEmpty();
    }
}
