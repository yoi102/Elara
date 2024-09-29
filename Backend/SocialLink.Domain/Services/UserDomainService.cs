using JWT;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SocialLink.Domain.Entities;
using SocialLink.Domain.Interfaces;
using SocialLink.Domain.Results;
using System.Security.Claims;

namespace SocialLink.Domain.Services
{
    public class UserDomainService : IUserDomainService
    {
        private readonly IEmailResetCodeValidator emailResetCodeValidator;
        private readonly IEmailSender emailSender;
        private readonly IOptions<JWTOptions> optJWT;
        private readonly ITokenService tokenService;
        private readonly IUserRepository userRepository;

        public UserDomainService(IUserRepository userRepository,
            ITokenService tokenService,
            IOptions<JWTOptions> optJWT,
            IEmailSender emailSender,
            IEmailResetCodeValidator emailResetCodeValidator)
        {
            this.userRepository = userRepository;
            this.tokenService = tokenService;
            this.optJWT = optJWT;
            this.emailSender = emailSender;
            this.emailResetCodeValidator = emailResetCodeValidator;
        }

        public async Task<IdentityResult> GetEmailResetCode(string email)
        {
            var user = await userRepository.FindByEmailAsync(email);
            if (user is null)
            {
                IdentityError error = new IdentityError { Description = "User not found" };
                return IdentityResult.Failed(error);
            }

            var resetCode = GenerateResetCode();
            await emailSender.SendEmailAsync(email, "Reset Code", resetCode);
            emailResetCodeValidator.StashEmailResetCode(email, resetCode);
            return IdentityResult.Success;
        }

        public async Task<LoginResult> LoginByEmailAndPasswordAsync(string email, string password)
        {
            (var checkResult, var user) = await CheckEmailAndPasswordAsync(email, password);
            return LoginAsync(checkResult, user);
        }

        public async Task<LoginResult> LoginByNameAndPasswordAsync(string name, string password)
        {
            (var checkResult, var user) = await CheckNameAndPasswordAsync(name, password);
            return LoginAsync(checkResult, user);
        }

        public async Task<IdentityResult> ResetPasswordByEmailResetCodeAsync(ResetPasswordRequest resetPasswordRequest)
        {
            if (!emailResetCodeValidator.Validate(resetPasswordRequest.Email, resetPasswordRequest.ResetCode))
            {
                IdentityError error = new IdentityError { Description = "Invalid or expired verification code" };
                return IdentityResult.Failed(error);
            }

            var identityResult = await userRepository.ResetPasswordByEmailAsync(resetPasswordRequest.Email, resetPasswordRequest.NewPassword);
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

            return tokenService.BuildToken(claims, optJWT.Value);
        }

        private async Task<(SignInResult, User?)> CheckCredentialsAsync(string password, User? user)
        {
            if (user == null)
            {
                return (SignInResult.Failed, user);
            }
            var result = await userRepository.CheckForSignInAsync(user, password, true);
            return (result, user);
        }

        private async Task<(SignInResult, User?)> CheckEmailAndPasswordAsync(string email, string password)
        {
            var user = await userRepository.FindByEmailAsync(email);
            return await CheckCredentialsAsync(password, user);
        }

        private async Task<(SignInResult, User?)> CheckNameAndPasswordAsync(string name, string password)
        {
            var user = await userRepository.FindByNameAsync(name);
            return await CheckCredentialsAsync(password, user);
        }

        private string GenerateResetCode()
        {
            Random random = new Random();
            string code = "";

            for (int i = 0; i < 5; i++)
            {
                code += random.Next(0, 10);
            }
            code = "123456";//固定值
            return code;
        }

        private LoginResult LoginAsync(SignInResult checkResult, User? user)
        {
            if (!checkResult.Succeeded)
            {
                return new LoginResult() { SignInResult = checkResult, Token = null };
            }
            else
            {
                string token = BuildToken(user!);
                return new LoginResult() { SignInResult = checkResult, Token = token };
            }
        }
    }
}