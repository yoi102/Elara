using JWT;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SocialLink.Domain.Entities;
using SocialLink.Domain.Results;
using System.Security.Claims;

namespace SocialLink.Domain.DomainService
{
    public class UserDomainService : IUserDomainService
    {
        private readonly IOptions<JWTOptions> optJWT;
        private readonly ITokenService tokenService;
        private readonly IUserRepository userRepository;

        public UserDomainService(IUserRepository userRepository, ITokenService tokenService, IOptions<JWTOptions> optJWT)
        {
            this.userRepository = userRepository;
            this.tokenService = tokenService;
            this.optJWT = optJWT;
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

        public async Task<LoginResult> LoginByPhoneNumberAndPasswordAsync(string phoneNumber, string password)
        {
            (var checkResult, var user) = await CheckPhoneAndPasswordAsync(phoneNumber, password);
            return LoginAsync(checkResult, user);
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

        private async Task<(SignInResult, User?)> CheckPhoneAndPasswordAsync(string phoneNumber, string password)
        {
            var user = await userRepository.FindByPhoneNumberAsync(phoneNumber);
            return await CheckCredentialsAsync(password, user);
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