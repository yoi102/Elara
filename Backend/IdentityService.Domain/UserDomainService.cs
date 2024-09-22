using DomainCommons.EntityStronglyIds;
using IdentityService.Domain.Data;
using IdentityService.Domain.Entities;
using IdentityService.Domain.Interfaces;
using IdentityService.Domain.Results;
using JWT;
using Microsoft.AspNetCore.Identity;

using System.Security.Claims;

namespace IdentityService.Domain;

public class UserDomainService : IUserDomainService
{
    private readonly ICacheService tokenCacheService;
    private readonly ITokenService tokenService;
    private readonly IUserRepository userRepository;

    public UserDomainService(IUserRepository userRepository,
                             ITokenService tokenService,
                             ICacheService tokenCacheService)
    {
        this.userRepository = userRepository;
        this.tokenService = tokenService;
        this.tokenCacheService = tokenCacheService;
    }

    public async Task<GetEmailResetCodeResult> GetResetCodeByEmail(string email)
    {
        var user = await userRepository.FindByEmailAsync(email);
        if (user is null)
        {
            IdentityError error = new IdentityError { Description = "User not found" };
            var identityResult = IdentityResult.Failed(error);

            return new GetEmailResetCodeResult(identityResult, email, "Reset Code", string.Empty);
        }
        var resetToken = await userRepository.GeneratePasswordResetTokenAsync(user);
        var resetCode = GenerateResetCode();
        var userId = user.Id;

        var resetCodeCacheData = new ResetCodeCacheData(userId, resetCode);

        await tokenCacheService.CacheResetCodeCacheAsync(resetCodeCacheData);

        return new GetEmailResetCodeResult(IdentityResult.Success, email, "Reset Code", resetCode);
    }

    public async Task<LoginResult> LoginByEmailAndPasswordAsync(string email, string password, string userAgent)
    {
        (var checkResult, var user) = await CheckEmailAndPasswordAsync(email, password);
        return await LoginAsync(checkResult, user, userAgent);
    }

    public async Task<LoginResult> LoginByNameAndPasswordAsync(string name, string password, string userAgent)
    {
        (var checkResult, var user) = await CheckNameAndPasswordAsync(name, password);
        return await LoginAsync(checkResult, user, userAgent);
    }

    public async Task<IdentityResult> ResetPasswordByResetCodeAsync(User user, string resetCode, string newPassword)
    {
        var resetCodeCache = await tokenCacheService.GetResetCodeCacheAsync(user.Id);
        if (resetCodeCache is null)
        {
            return IdentityResult.Failed(new IdentityError() { Description = "User not found", Code = "UserNotFound" });
        }
        if (resetCodeCache.ResetCode != resetCode)
        {
            return IdentityResult.Failed(new IdentityError
            {
                Code = "TokenMismatch",
                Description = "The reset token does not match the specified user."
            });
        }

        var identityResult = await userRepository.ResetPasswordAsync(user, newPassword);
        return identityResult;
    }

    private string BuildToken(User user)
    {
        if (user.UserName == null)
        {
            throw new ArgumentNullException(nameof(user.UserName), "UserName cannot be null");
        }
        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),

            new Claim(ClaimTypes.Name, user.UserName),
        };

        return tokenService.BuildToken(claims);
    }

    private async Task<(SignInResult, User?)> CheckEmailAndPasswordAsync(string email, string password)
    {
        var user = await userRepository.FindByEmailAsync(email);
        if (user == null)
        {
            return (SignInResult.Failed, user);
        }
        var result = await userRepository.CheckForSignInAsync(user, password, true);
        return (result, user);
    }

    private async Task<(SignInResult, User?)> CheckNameAndPasswordAsync(string name, string password)
    {
        var user = await userRepository.FindByNameAsync(name);
        if (user == null)
        {
            return (SignInResult.Failed, user);
        }
        var result = await userRepository.CheckForSignInAsync(user, password, true);
        return (result, user);
    }

    private async Task<LoginResult> LoginAsync(SignInResult checkResult, User? user, string userAgent)
    {
        if (!checkResult.Succeeded || user is null)
        {
            return new LoginResult() { SignInResult = checkResult, };
        }
        else
        {
            string token = BuildToken(user);
            var refreshToken = tokenService.GenerateRefreshToken();
            var newRefreshTokenData = new RefreshTokenData(user.Id, refreshToken, userAgent);
            await tokenCacheService.CacheRefreshTokenCacheAsync(newRefreshTokenData);
            return new LoginResult() { SignInResult = checkResult, Token = token, RefreshToken = refreshToken, UserId = user.Id, UserName = user.UserName };
        }
    }

    public async Task<RefreshTokenResult> RefreshTokenAsync(UserId userId, string refreshToken, string userAgent)
    {
        var savedTokenData = await tokenCacheService.GetRefreshTokenCacheAsync(refreshToken);
        if (savedTokenData is null)
            return new RefreshTokenResult(default, default, default, RefreshTokenStatus.InvalidToken);

        if (savedTokenData.UserId != userId)
            return new RefreshTokenResult(default, default, default, RefreshTokenStatus.TokenMismatch);

        var user = await userRepository.FindByIdAsync(userId);
        if (user is null)
            return new RefreshTokenResult(default, default, default, RefreshTokenStatus.UserNotFound);

        var token = BuildToken(user);

        var newRefreshToken = tokenService.GenerateRefreshToken();
        var newRefreshTokenData = new RefreshTokenData(userId, newRefreshToken, userAgent);
        await tokenCacheService.CacheRefreshTokenCacheAsync(newRefreshTokenData);

        await tokenCacheService.RemoveAsync(refreshToken);

        return new RefreshTokenResult(user.UserName, token, newRefreshToken, RefreshTokenStatus.Success);
    }

    private string GenerateResetCode()
    {
        var random = new Random();
        var number = random.Next(0, 1000000); // 从 0 到 999999
        return number.ToString("D6"); // 始终补齐 6 位，不足补 0
    }
}
