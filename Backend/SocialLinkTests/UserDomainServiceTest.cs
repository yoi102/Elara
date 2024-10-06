using FluentAssertions;
using JWT;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Extensions.Options;
using Moq;
using SocialLink.Domain.Entities;
using SocialLink.Domain.Interfaces;
using SocialLink.Domain.Services;
using System.Security.Claims;

namespace SocialLinkTests
{
    public class UserDomainServiceTest
    {
        private readonly Mock<IEmailResetCodeValidator> _emailResetCodeValidatorMock;
        private readonly Mock<IOptions<JWTOptions>> _optJWTMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly UserDomainService _userDomainService;
        private readonly Mock<IUserRepository> _userRepositoryMock;

        public UserDomainServiceTest()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _tokenServiceMock = new Mock<ITokenService>();
            _optJWTMock = new Mock<IOptions<JWTOptions>>();
            _emailResetCodeValidatorMock = new Mock<IEmailResetCodeValidator>();
            _userDomainService = new UserDomainService(_userRepositoryMock.Object,
                                              _tokenServiceMock.Object,
                                                 _optJWTMock.Object,
                                      _emailResetCodeValidatorMock.Object);
        }

        [Fact]
        public async Task GetEmailResetCode_Should_ReturnSuccess()
        {
            var user = new User("name", "email@123.com");
            _userRepositoryMock.Setup(ur => ur.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
            _emailResetCodeValidatorMock.Setup(ev => ev.StashEmailResetCode(It.IsAny<string>(), It.IsAny<string>()));

            var result = await _userDomainService.GetEmailResetCode(user.Email!);

            _userRepositoryMock.Verify(ur => ur.FindByEmailAsync(user.Email!));
            _emailResetCodeValidatorMock.Verify(ev => ev.StashEmailResetCode(user.Email!, It.IsAny<string>()));

            result.Should().NotBeNull();
            result.IdentityResult.Succeeded.Should().BeTrue();
            result.Email.Should().Be(user.Email!);
            result.HtmlMessage.Should().BeOfType<string>();
        }

        [Fact]
        public async Task LoginByEmailAndPasswordAsync_Should_ReturnSuccess()
        {
            var user = new User("name", "email@123.com");
            user.PasswordHash = "12345678";

            _userRepositoryMock.Setup(ur => ur.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
            _userRepositoryMock.Setup(ur => ur.CheckForSignInAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(SignInResult.Success);
            _tokenServiceMock.Setup(ts => ts.BuildToken(It.IsAny<List<Claim>>(), It.IsAny<JWTOptions>())).Returns("token");

            var result = await _userDomainService.LoginByEmailAndPasswordAsync(user.Email!, user.PasswordHash);

            _userRepositoryMock.Verify(ur => ur.FindByEmailAsync(user.Email!));
            _userRepositoryMock.Verify(ur => ur.CheckForSignInAsync(user, user.PasswordHash!, true));

            result.Should().NotBeNull();
            result.SignInResult.Should().Be(SignInResult.Success);
            result.Token.Should().BeOfType<string>();
        }

        [Fact]
        public async Task LoginByNameAndPasswordAsync_Should_ReturnSuccess()
        {
            var user = new User("name", "email@123.com");
            user.PasswordHash = "12345678";

            _userRepositoryMock.Setup(ur => ur.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(user);
            _userRepositoryMock.Setup(ur => ur.CheckForSignInAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(SignInResult.Success);
            _tokenServiceMock.Setup(ts => ts.BuildToken(It.IsAny<List<Claim>>(), It.IsAny<JWTOptions>())).Returns("token");

            var result = await _userDomainService.LoginByNameAndPasswordAsync(user.UserName!, user.PasswordHash);

            _userRepositoryMock.Verify(ur => ur.FindByNameAsync(user.UserName!));
            _userRepositoryMock.Verify(ur => ur.CheckForSignInAsync(user, user.PasswordHash!, true));

            result.Should().NotBeNull();
            result.SignInResult.Should().Be(SignInResult.Success);
            result.Token.Should().BeOfType<string>();
        }

        [Fact]
        public async Task ResetPasswordByEmailResetCodeAsync_Should_ReturnSuccess()
        {
            var resetPasswordRequest = new ResetPasswordRequest()
            {
                Email = "1231@123.com",
                ResetCode = "123",
                NewPassword = "14567"
            };

            _emailResetCodeValidatorMock.Setup(ev => ev.Validate(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            _userRepositoryMock.Setup(ur => ur.ResetPasswordByEmailAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            var result = await _userDomainService.ResetPasswordByEmailResetCodeAsync(resetPasswordRequest);

            _emailResetCodeValidatorMock.Verify(ev => ev.Validate(resetPasswordRequest.Email, resetPasswordRequest.ResetCode));
            _userRepositoryMock.Verify(ur => ur.ResetPasswordByEmailAsync(resetPasswordRequest.Email, resetPasswordRequest.NewPassword));

            result.Should().Be(IdentityResult.Success);
        }
    }
}