using DomainCommons.EntityStronglyIds;
using Microsoft.AspNetCore.Identity;
using SocialLink.Domain.Entities;
using SocialLink.Domain.Interfaces;
using SocialLink.Domain.Results;

namespace SocialLink.infrastructure
{
    public class UserRepository : IUserRepository
    {
        private readonly IdUserManager userManager;

        public UserRepository(IdUserManager userManager)
        {
            this.userManager = userManager;
        }

        public Task<IdentityResult> AccessFailedAsync(User user)
        {
            return userManager.AccessFailedAsync(user);
        }

        public async Task<IdentityResult> ChangePasswordAsync(UserId userId, string newPassword)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                IdentityError error = new IdentityError();
                error.Code = "Not Found";
                error.Description = "User not found";
                return IdentityResult.Failed(error);
            }
            var passwordValidators = userManager.PasswordValidators;
            foreach (var validator in passwordValidators)
            {
                var result = await validator.ValidateAsync(userManager, user, newPassword);
                if (!result.Succeeded)
                {
                    return IdentityResult.Failed(result.Errors.ToArray());
                }
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

        public Task<User?> FindByEmailAsync(string email)
        {
            return userManager.FindByEmailAsync(email);
        }

        public Task<User?> FindByIdAsync(UserId userId)
        {
            return userManager.FindByIdAsync(userId.ToString());
        }

        public Task<User?> FindByNameAsync(string name)
        {
            return userManager.FindByNameAsync(name);
        }

        public async Task<IdentityResult> RemoveUserAsync(UserId id)
        {
            var user = await FindByIdAsync(id);
            if (user == null)
            {
                return IdentityResult.Success;
            }
            var userLoginStore = userManager.UserLoginStore;
            var logins = await userLoginStore.GetLoginsAsync(user, default);
            foreach (var login in logins)
            {
                await userLoginStore.RemoveLoginAsync(user, login.LoginProvider, login.ProviderKey, default);
            }
            user.SoftDelete();
            var result = await userManager.UpdateAsync(user);
            return result;
        }

        public async Task<IdentityResult> ResetPasswordByEmailAsync(string email, string newPassword)
        {
            var user = await FindByEmailAsync(email);

            if (user == null)
            {
                return ErrorResult("User not found");
            }
            if (await userManager.CheckPasswordAsync(user, newPassword))
            {
                return ErrorResult("New password cannot be the same as the old password.");
            }
            string token = await userManager.GeneratePasswordResetTokenAsync(user);

            return await userManager.ResetPasswordAsync(user, token, newPassword);
        }

        public async Task<IdentityResult> ResetPasswordByIdAsync(UserId id, string newPassword)
        {
            var user = await FindByIdAsync(id);

            if (user == null)
            {
                return ErrorResult("User not found");
            }
            if (await userManager.CheckPasswordAsync(user, newPassword))
            {
                return ErrorResult("New password cannot be the same as the old password.");
            }
            string token = await userManager.GeneratePasswordResetTokenAsync(user);

            return await userManager.ResetPasswordAsync(user, token, newPassword);
        }

        public async Task<SignUpResult> SignUpAsync(string name, string email, string password)
        {
            var user = new User(name, email);

            var identityResult = await this.userManager.CreateAsync(user, password);

            var result = new SignUpResult() { IdentityResult = identityResult };

            if (!result.Succeeded)
                return result;

            var createdUser = await this.userManager.FindByNameAsync(name);
            if (createdUser == null)
            {
                result.IdentityResult = IdentityResult.Failed();
                return result;
            }

            result.User = createdUser;
            return result;
        }

        private static IdentityResult ErrorResult(string msg)
        {
            IdentityError idError = new IdentityError { Description = msg };
            return IdentityResult.Failed(idError);
        }
    }
}