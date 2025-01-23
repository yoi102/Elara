using FluentValidation;

namespace IdentityService.WebAPI.Controllers.User.Request;

public record class LoginByNameAndPasswordRequest(string Name, string Password);

public class LoginByNameAndPasswordRequestValidator : AbstractValidator<LoginByNameAndPasswordRequest>
{
    public LoginByNameAndPasswordRequestValidator()
    {
        RuleFor(e => e.Name).NotNull().NotEmpty();
        RuleFor(e => e.Password).NotNull().NotEmpty();
    }
}