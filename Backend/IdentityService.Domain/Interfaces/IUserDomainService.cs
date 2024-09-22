using DomainCommons.EntityStronglyIds;
using IdentityService.Domain.Entities;
using IdentityService.Domain.Results;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Domain.Interfaces;

public interface IUserDomainService
{
    Task<GetEmailResetCodeResult> GetResetCodeByEmail(string email);

    Task<LoginResult> LoginByEmailAndPasswordAsync(string email, string password,string userAgent);

    Task<LoginResult> LoginByNameAndPasswordAsync(string name, string password,string userAgent);

    Task<IdentityResult> ResetPasswordByResetCodeAsync(User user, string resetCode, string newPassword);

    Task<RefreshTokenResult> RefreshTokenAsync(UserId userId, string refreshToken, string userAgent);
}
