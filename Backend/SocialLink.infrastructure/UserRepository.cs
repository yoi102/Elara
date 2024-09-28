using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SocialLink.Domain;
using SocialLink.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLink.infrastructure
{
    public class UserRepository : IUserRepository
    {
        private readonly IdUserManager userManager;
        private readonly ILogger<UserRepository> logger;

        public UserRepository(IdUserManager userManager, ILogger<UserRepository> logger)
        {
            this.userManager = userManager;
            this.logger = logger;
        }
        public Task<IdentityResult> AccessFailedAsync(User user)
        {
            return userManager.AccessFailedAsync(user);
        }

        public async Task<IdentityResult> ChangePasswordAsync(UserId userId, string newPassword)
        {
            if (newPassword.Length < 6)
            {
                IdentityError error = new IdentityError();
                error.Code = "Password Invalid";
                error.Description = "密码长度不能少于6";
                return IdentityResult.Failed(error);
            }
            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                IdentityError error = new IdentityError();
                error.Code = "Not Found";
                error.Description = "没有找到用户";
                return IdentityResult.Failed(error);
            }
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var resetPasswordResult = await userManager.ResetPasswordAsync(user, token, newPassword);
            return resetPasswordResult;
        }

        public async Task<SignInResult> CheckForSignInAsync(User user, string password, bool lockoutOnFailure)
        {
            if (await userManager.IsLockedOutAsync(user))
            {
                return SignInResult.LockedOut;
            }
            
            var success = await userManager.CheckPasswordAsync(user, password);
            if (success)
            {
                return SignInResult.Success;
            }
            else
            {
                if (!lockoutOnFailure)
                {
                    var r = await AccessFailedAsync(user);
                    if (!r.Succeeded)
                    {
                        throw new ApplicationException("AccessFailed failed");
                    }
                }
                return SignInResult.Failed;
            }
        }

        public async Task<(IdentityResult, User)> RegisterAsync(string name, string password)
        {
            var user = new User(name);

            return (await this.userManager.CreateAsync(user, password), user);
        }

        public Task<User?> FindByIdAsync(UserId userId)
        {
            return userManager.FindByIdAsync(userId.ToString());
        }

        public Task<User?> FindByNameAsync(string name)
        {
            return userManager.FindByNameAsync(name);
        }

        public Task<User?> FindByPhoneEmailAsync(string email)
        {
            return userManager.FindByEmailAsync(email);
        }

        public Task<User?> FindByPhoneNumberAsync(string phoneNumber)
        {
            return userManager.Users.FirstOrDefaultAsync(user => user.PhoneNumber == phoneNumber);

        }

        public async Task<IdentityResult> RemoveUserAsync(UserId id)
        {
            var user = await FindByIdAsync(id);
            var userLoginStore = userManager.UserLoginStore;
            //一定要删除 aspnetuserlogins 表中的数据，否则再次用这个外部登录登录的话
            //就会报错：The instance of entity type 'IdentityUserLogin<Guid>' cannot be tracked because another instance with the same key value for {'LoginProvider', 'ProviderKey'} is already being tracked.
            //而且要先删除 aspnetuserlogins 数据，再软删除User
            if (user == null)
            {
                return IdentityResult.Success;
            }
            var logins = await userLoginStore.GetLoginsAsync(user, default);
            foreach (var login in logins)
            {
                await userLoginStore.RemoveLoginAsync(user, login.LoginProvider, login.ProviderKey, default);
            }
            user.SoftDelete();
            var result = await userManager.UpdateAsync(user);
            return result;
        }
    }
}
