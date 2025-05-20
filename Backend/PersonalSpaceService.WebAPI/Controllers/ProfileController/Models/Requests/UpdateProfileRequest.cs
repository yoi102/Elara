using ASPNETCore;
using DomainCommons.EntityStronglyIds;
using FluentValidation;

namespace PersonalSpaceService.WebAPI.Controllers.ProfileController.Models.Requests;

public record class UpdateProfileRequest(string DisplayName, UploadedItemId AvatarItemId);

public class LoginByEmailAndPasswordRequestValidator : AbstractValidator<UpdateProfileRequest>
{
    public LoginByEmailAndPasswordRequestValidator()
    {
        RuleFor(e => e.DisplayName).NotNull().NotEmpty();
        RuleFor(e => e.AvatarItemId).NotEmpty();
    }
}
