using FluentValidation;

namespace SocialLink.WebAPI.Controllers.User.Request
{
 

    public record class LoginByEmailAndPasswordRequest(string Email, string Password);



    public class LoginByEmailAndPasswordRequestValidator : AbstractValidator<LoginByEmailAndPasswordRequest>
    {
        public LoginByEmailAndPasswordRequestValidator()
        {
            RuleFor(e => e.Email).NotNull().NotEmpty().EmailAddress();
            RuleFor(e => e.Password).NotNull().NotEmpty();
        }
    }
}
