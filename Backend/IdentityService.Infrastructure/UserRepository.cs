using DomainCommons.EntityStronglyIds;
using IdentityService.Domain.Entities;
using IdentityService.Domain.Interfaces;
using IdentityService.Domain.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure;

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

    public Task<string> GeneratePasswordResetTokenAsync(User user)
    {
        return userManager.GeneratePasswordResetTokenAsync(user);
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

    public async Task<IdentityResult> ResetPasswordAsync(User user, string newPassword)
    {
        string token = await userManager.GeneratePasswordResetTokenAsync(user);
        return await userManager.ResetPasswordAsync(user, token, newPassword);
    }

    public async Task<IdentityResult> ResetPasswordByOldPasswordAsync(UserId id, string oldPassword, string newPassword)
    {
        var user = await FindByIdAsync(id);

        if (user == null)
        {
            return ErrorResult("User not found");
        }
        if (!await userManager.CheckPasswordAsync(user, oldPassword))
        {
            return ErrorResult("Incorrect original password.");
        }

        string token = await userManager.GeneratePasswordResetTokenAsync(user);

        return await userManager.ResetPasswordAsync(user, token, newPassword);
    }

    public async Task<IdentityResult> ResetPasswordByResetTokenAsync(User user, string newPassword, string resetToken)
    {
        if (await userManager.CheckPasswordAsync(user, newPassword))
        {
            return ErrorResult("New password cannot be the same as the old password.", "SamePassword");
        }

        return await userManager.ResetPasswordAsync(user, resetToken, newPassword);
    }

    public async Task<User[]> SearchUsersByNameAsync(string partialName)
    {
        return await userManager.Users
                                     .Where(user => !string.IsNullOrEmpty(user.UserName) && user.UserName.Contains(partialName))
                                     .ToArrayAsync();
    }

    public async Task<SignUpResult> SignUpAsync(string name, string email, string password)
    {
        var user = new User(name, email);

        var identityResult = await userManager.CreateAsync(user, password);

        var result = new SignUpResult() { IdentityResult = identityResult };

        if (!result.Succeeded)
            return result;

        var createdUser = await userManager.FindByNameAsync(name);
        if (createdUser == null)
        {
            result.IdentityResult = IdentityResult.Failed();
            return result;
        }

        result.User = createdUser;
        return result;
    }

    private static IdentityResult ErrorResult(string description, string code = "")
    {
        IdentityError error = new IdentityError { Description = description, Code = code };
        return IdentityResult.Failed(error);
    }
}
