using DomainCommons.EntityStronglyIds;
using FluentValidation;

namespace ChatService.WebAPI.Controllers.GroupConversationController.Requests;


public record GroupConversationCreateRequest(string Name, GroupConversationAddMemberRequest[] Member);

public class GroupConversationCreateRequestValidator : AbstractValidator<GroupConversationCreateRequest>
{
    public GroupConversationCreateRequestValidator()
    {
        RuleFor(e => e.Name).NotNull().NotEmpty();
        RuleFor(e => e.Member).NotNull();
    }
}



