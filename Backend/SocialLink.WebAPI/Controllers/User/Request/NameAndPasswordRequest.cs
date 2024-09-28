using FluentValidation;

namespace SocialLink.WebAPI.Controllers.User.Request
{
    public record class NameAndPasswordRequest(string Name, string Password);



    public class LoginByNameAndPasswordRequestValidator : AbstractValidator<NameAndPasswordRequest>
    {
        public LoginByNameAndPasswordRequestValidator()
        {
            RuleFor(e => e.Name).NotNull().NotEmpty();
            RuleFor(e => e.Password).NotNull().NotEmpty();
        }
    }

}
