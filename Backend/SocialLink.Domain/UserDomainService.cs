using JWT;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SocialLink.Domain.Entities;
using System.Security.Claims;

namespace SocialLink.Domain
{
    public class UserDomainService
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

        public async Task<(SignInResult Result, string? Token)> LoginByNameAndPasswordAsync(string name, string password)
        {
            (var checkResult, var user) = await CheckNameAndPasswordAsync(name, password);
            if (checkResult.Succeeded)
            {
                string token = BuildToken(user!);
                return (SignInResult.Success, token);
            }
            else
            {
                return (checkResult, null);
            }
        }

        private string BuildToken(User user)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName!),
            };

            return tokenService.BuildToken(claims, optJWT.Value);
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
    }
}