using DomainCommons.EntityStronglyIds;
using IdentityService.Domain.Entities;
using IdentityService.Domain.Results;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Domain.Interfaces;

public interface IUserRepository
{
    Task<IdentityResult> AccessFailedAsync(User user);

    Task<IdentityResult> ChangePasswordAsync(UserId userId, string newPassword);

    Task<SignInResult> CheckForSignInAsync(User user, string password, bool lockoutOnFailure);

    Task<User?> FindByEmailAsync(string email);

    Task<User?> FindByIdAsync(UserId userId);

    Task<User?> FindByNameAsync(string name);

    Task<string> GeneratePasswordResetTokenAsync(User user);

    Task<IdentityResult> RemoveUserAsync(UserId id);

    Task<IdentityResult> ResetPasswordAsync(User user, string newPassword);

    Task<IdentityResult> ResetPasswordByOldPasswordAsync(UserId id, string oldPassword, string newPassword);

    Task<IdentityResult> ResetPasswordByResetTokenAsync(User user, string newPassword, string resetToken);

    Task<User[]> SearchUsersByNameAsync(string partialName);

    Task<SignUpResult> SignUpAsync(string name, string email, string password);
}
