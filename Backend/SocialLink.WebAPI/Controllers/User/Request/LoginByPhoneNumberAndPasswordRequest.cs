using FluentValidation;

namespace SocialLink.WebAPI.Controllers.User.Request
{
   

    public record class LoginByPhoneNumberAndPasswordRequest(string PhoneNumber, string Password);



    public class LoginByPhoneNumberAndPasswordRequestValidator : AbstractValidator<LoginByPhoneNumberAndPasswordRequest>
    {
        public LoginByPhoneNumberAndPasswordRequestValidator()
        {
            RuleFor(e => e.PhoneNumber).NotNull().NotEmpty();
            RuleFor(e => e.Password).NotNull().NotEmpty();
        }
    }
}
