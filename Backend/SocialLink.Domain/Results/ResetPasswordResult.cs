using Microsoft.AspNetCore.Identity;

namespace SocialLink.Domain.Results
{
    public record class ResetPasswordResult
    {
        public required IdentityResult IdentityResult { get; set; }
    }
}
