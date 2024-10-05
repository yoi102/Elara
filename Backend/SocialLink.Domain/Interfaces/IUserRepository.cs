using Microsoft.AspNetCore.Identity;
using SocialLink.Domain.Entities;
using SocialLink.Domain.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLink.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<IdentityResult> AccessFailedAsync(User user);

        Task<IdentityResult> ChangePasswordAsync(UserId userId, string newPassword);

        Task<SignInResult> CheckForSignInAsync(User user, string password, bool lockoutOnFailure);

        Task<User?> FindByEmailAsync(string email);

        Task<User?> FindByIdAsync(UserId userId);

        Task<User?> FindByNameAsync(string name);

        Task<SignUpResult> SignUpAsync(string name,string email, string password);

        Task<IdentityResult> RemoveUserAsync(UserId id);

        Task<IdentityResult> ResetPasswordByEmailAsync(string email, string newPassword);
        Task<IdentityResult> ResetPasswordByIdAsync(UserId id, string newPassword);
    }
}