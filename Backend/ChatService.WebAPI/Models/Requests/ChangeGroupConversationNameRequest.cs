using DomainCommons.EntityStronglyIds;
using FluentValidation;

namespace ChatService.WebAPI.Models.Requests;


public record ChangeGroupConversationNameRequest(ConversationId Id, string NewName);

public class ChangeConversationNameRequestValidator : AbstractValidator<ChangeGroupConversationNameRequest>
{
    public ChangeConversationNameRequestValidator()
    {
        RuleFor(e => e.Id.Value).NotEmpty();
        RuleFor(e => e.NewName).NotNull().NotEmpty();
    }
}
