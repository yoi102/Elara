using Microsoft.AspNetCore.Identity;
using SocialLink.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLink.Domain
{
    public interface IUserRepository
    {
        Task<IdentityResult> AccessFailedAsync(User user);

        Task<IdentityResult> ChangePasswordAsync(UserId userId, string newPassword);

        Task<SignInResult> CheckForSignInAsync(User user, string password, bool lockoutOnFailure);

        Task<(IdentityResult,User)> RegisterAsync(string name, string password);

        Task<User?> FindByIdAsync(UserId userId);

        Task<User?> FindByNameAsync(string name);

        Task<User?> FindByPhoneEmailAsync(string email);

        Task<User?> FindByPhoneNumberAsync(string phoneNumber);
        Task<IdentityResult> RemoveUserAsync(UserId id);
    }
}