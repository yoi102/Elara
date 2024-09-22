using Microsoft.AspNetCore.Identity;

namespace IdentityService.Domain.Results;

public record class ResetPasswordResult
{
    public required IdentityResult IdentityResult { get; set; }
}