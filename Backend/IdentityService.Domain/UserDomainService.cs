using IdentityService.Domain.Entities;
using IdentityService.Domain.Interfaces;
using IdentityService.Domain.Results;
using JWT;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Extensions.Options;

using System.Security.Claims;

namespace IdentityService.Domain
{
    public class UserDomainService : IUserDomainService
    {
        private readonly IEmailResetCodeValidator emailResetCodeValidator;
        private readonly IOptions<JWTOptions> optJWT;
        private readonly ITokenService tokenService;
        private readonly IUserRepository userRepository;

        public UserDomainService(IUserRepository userRepository,
            ITokenService tokenService,
            IOptions<JWTOptions> optJWT,
            IEmailResetCodeValidator emailResetCodeValidator)
        {
            this.userRepository = userRepository;
            this.tokenService = tokenService;
            this.optJWT = optJWT;
            this.emailResetCodeValidator = emailResetCodeValidator;
        }

        public async Task<GetEmailResetCodeResult> GetEmailResetCode(string email)
        {
            var user = await userRepository.FindByEmailAsync(email);
            if (user is null)
            {
                IdentityError error = new IdentityError { Description = "User not found" };
                var identityResult = IdentityResult.Failed(error);

                return new GetEmailResetCodeResult(identityResult, email, "Reset Code", string.Empty);
            }

            var resetCode = GenerateResetCode();
            emailResetCodeValidator.StashEmailResetCode(email, resetCode);
            return new GetEmailResetCodeResult(IdentityResult.Success, email, "Reset Code", resetCode);
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