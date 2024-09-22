using ASPNETCore;
using DomainCommons.EntityStronglyIds;
using FluentValidation;

namespace PersonalSpaceService.WebAPI.Controllers.ProfileController.Requsets;

public record class UpdateProfileRequest(string DisplayName, UploadedItemId Avatar);



public class LoginByEmailAndPasswordRequestValidator : AbstractValidator<UpdateProfileRequest>
{
    public LoginByEmailAndPasswordRequestValidator()
    {
        RuleFor(e => e.DisplayName).NotNull().NotEmpty();
        RuleFor(e => e.Avatar).NotEmpty();
    }
}
