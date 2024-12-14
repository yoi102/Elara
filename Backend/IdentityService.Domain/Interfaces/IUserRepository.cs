using DomainCommons.EntityStronglyIds;
using IdentityService.Domain.Entities;
using IdentityService.Domain.Results;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<string> GeneratePasswordResetTokenAsync(User user);
        Task<IdentityResult> AccessFailedAsync(User user);

        Task<IdentityResult> ChangePasswordAsync(UserId userId, string newPassword);

        Task<SignInResult> CheckForSignInAsync(User user, string password, bool lockoutOnFailure);

        Task<User?> FindByEmailAsync(string email);

        Task<User?> FindByIdAsync(UserId userId);

        Task<User?> FindByNameAsync(string name);

        Task<IdentityResult> RemoveUserAsync(UserId id);

        Task<IdentityResult> ResetPasswordByEmailAsync(string email, string newPassword, string token);

        Task<IdentityResult> ResetPasswordByIdAsync(UserId id, string oldPassword, string newPassword);

        Task<User[]> SearchUsersByNameAsync(string partialName);

        Task<SignUpResult> SignUpAsync(string name, string email, string password);
    }
}