using FluentValidation;
using Microsoft.AspNetCore.Identity.Data;

namespace IdentityService.WebAPI.Controllers.User.Request;

public record class ResetPasswordRequestRequest(string Name, string Password);

public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
{
    public ResetPasswordRequestValidator()
    {
        RuleFor(e => e.ResetCode).NotNull().NotEmpty();
        RuleFor(e => e.NewPassword).NotNull().NotEmpty();
        RuleFor(e => e.Email).NotNull().NotEmpty().EmailAddress();
    }
}