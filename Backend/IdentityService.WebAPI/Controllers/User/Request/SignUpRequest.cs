using FluentValidation;

namespace IdentityService.WebAPI.Controllers.User.Request;

public record class SignUpRequest(string Name, string Email, string Password);

public class SignUpRequestValidator : AbstractValidator<SignUpRequest>
{
    public SignUpRequestValidator()
    {
        RuleFor(e => e.Email).NotNull().NotEmpty().EmailAddress();
        RuleFor(e => e.Name).NotNull().NotEmpty();
        RuleFor(e => e.Password).NotNull().NotEmpty().MinimumLength(6).Matches(@"^[\u0020-\u007E]+$");
    }
}
