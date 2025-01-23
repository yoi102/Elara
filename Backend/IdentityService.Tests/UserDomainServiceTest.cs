using FluentAssertions;
using IdentityService.Domain;
using IdentityService.Domain.Entities;
using IdentityService.Domain.Interfaces;
using JWT;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Extensions.Options;
using Moq;
using System.Security.Claims;

namespace IdentityService.Tests;

public class UserDomainServiceTest
{
    private readonly Mock<IResetTokenCacheService> _resetTokenCacheService;
    private readonly Mock<IOptions<JWTOptions>> _optJWTMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly UserDomainService _userDomainService;
    private readonly Mock<IUserRepository> _userRepositoryMock;

    public UserDomainServiceTest()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _tokenServiceMock = new Mock<ITokenService>();
        _optJWTMock = new Mock<IOptions<JWTOptions>>();
        _resetTokenCacheService = new Mock<IResetTokenCacheService>();
        _userDomainService = new UserDomainService(_userRepositoryMock.Object,
                                          _tokenServiceMock.Object,
                                             _optJWTMock.Object,
                                  _resetTokenCacheService.Object);
    }

    [Fact]
    public async Task GetEmailResetCode_Should_ReturnSuccess()
    {
        var user = new User("name", "email@123.com");
        _userRepositoryMock.Setup(ur => ur.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
        _resetTokenCacheService.Setup(ev => ev.CacheToken(It.IsAny<string>())).Returns(It.IsAny<string>());

        var result = await _userDomainService.GetEmailResetCode(user.Email!);

        _userRepositoryMock.Verify(ur => ur.FindByEmailAsync(user.Email!));
        _resetTokenCacheService.Verify(ev => ev.CacheToken(It.IsAny<string>()));

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

        _resetTokenCacheService.Setup(ev => ev.FindTokenByResetCode(It.IsAny<string>())).Returns(It.IsAny<string>());
        _userRepositoryMock.Setup(ur => ur.ResetPasswordByEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

        var result = await _userDomainService.ResetPasswordByEmailResetCodeAsync(resetPasswordRequest);

        _resetTokenCacheService.Verify(ev => ev.FindTokenByResetCode(resetPasswordRequest.ResetCode));
        _userRepositoryMock.Verify(ur => ur.ResetPasswordByEmailAsync(resetPasswordRequest.Email, resetPasswordRequest.NewPassword, It.IsAny<string>()));

        result.Should().Be(IdentityResult.Success);
    }
}
