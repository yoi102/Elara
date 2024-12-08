using ASPNETCore;
using FluentValidation;

namespace PersonalSpaceService.WebAPI.Controllers.Request
{
    public record class UpdateProfileRequest(string DisplayName, Uri Avatar);
   


    public class LoginByEmailAndPasswordRequestValidator : AbstractValidator<UpdateProfileRequest>
    {
        public LoginByEmailAndPasswordRequestValidator()
        {
            RuleFor(e => e.DisplayName).NotNull().NotEmpty();
            RuleFor(e => e.Avatar).Length(5, 500);//Avatar允许为空
        }
    }

}
