using FluentValidation;

namespace IdentityService.WebAPI.Controllers.User.Request;

public record class LoginByEmailAndPasswordRequest(string Email, string Password, string UserAgent);

public class LoginByEmailAndPasswordRequestValidator : AbstractValidator<LoginByEmailAndPasswordRequest>
{
    public LoginByEmailAndPasswordRequestValidator()
    {
        RuleFor(e => e.Email).NotNull().NotEmpty().EmailAddress();
        RuleFor(e => e.Password).NotNull().NotEmpty();
        RuleFor(e => e.UserAgent).NotNull().NotEmpty();
    }
}
