using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using SocialLink.Domain.Results;

namespace SocialLink.Domain.Interfaces
{
    public interface IUserDomainService
    {
        Task<GetEmailResetCodeResult> GetEmailResetCode(string email);
        Task<LoginResult> LoginByEmailAndPasswordAsync(string email, string password);
        Task<LoginResult> LoginByNameAndPasswordAsync(string name, string password);
        Task<IdentityResult> ResetPasswordByEmailResetCodeAsync(ResetPasswordRequest resetPasswordRequest);
    }
}
