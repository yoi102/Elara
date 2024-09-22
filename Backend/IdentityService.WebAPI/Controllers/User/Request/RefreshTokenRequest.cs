using DomainCommons.EntityStronglyIds;
using FluentValidation;

namespace IdentityService.WebAPI.Controllers.User.Request;

public record class RefreshTokenRequest(UserId UserId, string RefreshToken, string UserAgent);

public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
{
    public RefreshTokenRequestValidator()
    {
        RuleFor(e => e.UserId).Must(id => id != UserId.Empty);
        RuleFor(e => e.RefreshToken).NotNull().NotEmpty();
        RuleFor(e => e.UserAgent).NotNull().NotEmpty();
    }
}
