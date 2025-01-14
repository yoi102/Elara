using DomainCommons.EntityStronglyIds;
using FluentValidation;

namespace ChatService.WebAPI.Controllers.GroupConversationController.Requests;

public record GroupConversationAddMemberRequest(UserId UserId, string Role);

public class GroupConversationAddMemberRequestValidator : AbstractValidator<GroupConversationAddMemberRequest>
{
    public GroupConversationAddMemberRequestValidator()
    {
        RuleFor(e => e.UserId.Value).NotEmpty();
        RuleFor(e => e.Role).NotNull().NotEmpty();
    }
}
