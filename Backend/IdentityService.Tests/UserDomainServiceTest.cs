using DomainCommons.EntityStronglyIds;
using FluentAssertions;
using IdentityService.Domain;
using IdentityService.Domain.Data;
using IdentityService.Domain.Entities;
using IdentityService.Domain.Interfaces;
using IdentityService.Infrastructure;
using JWT;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using System.Security.Claims;

namespace IdentityService.Tests;
//由于代码重构、后续需待重新审视

public class UserDomainServiceTest
{
    private readonly IdentityDbContext _context;
    private readonly Mock<ICacheService> _resetTokenCacheService;
    private readonly Mock<IOptions<JWTOptions>> _optJWTMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly UserDomainService _userDomainService;
    private readonly Mock<IUserRepository> _userRepositoryMock;

    public UserDomainServiceTest()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _tokenServiceMock = new Mock<ITokenService>();
        _optJWTMock = new Mock<IOptions<JWTOptions>>();
        _resetTokenCacheService = new Mock<ICacheService>();
        _userDomainService = new UserDomainService(_userRepositoryMock.Object,
                                          _tokenServiceMock.Object,
                                  _resetTokenCacheService.Object);
        _context = GetDataBaseContext().Result;
    }

    private async Task<IdentityDbContext> GetDataBaseContext()
    {
        var mediatorMock = new Mock<IMediator>();

        var options = new DbContextOptionsBuilder<IdentityDbContext>()
                                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                                .Options;
        var dbContext = new IdentityDbContext(options, mediatorMock.Object);
        await dbContext.Database.EnsureCreatedAsync();
        if (await dbContext.Users.CountAsync() <= 0)
        {
            for (int i = 0; i < 10; i++)
            {
                dbContext.Users.Add(new IdentityService.Domain.Entities.User(i.ToString(), $"{i}123@1mail.com"));
            }

            await dbContext.SaveChangesAsync();
        }

        return dbContext;
    }

    [Fact]
    public async Task GetResetCodeByEmail_Should_ReturnSuccess()
    {
        var user = new User("name", "email@123.com");
        var resetCodeCacheData = new ResetCodeCacheData(user.Id, "111111");
        _userRepositoryMock.Setup(ur => ur.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
        _userRepositoryMock.Setup(ur => ur.GeneratePasswordResetTokenAsync(user)).ReturnsAsync(resetCodeCacheData.ResetCode);
        _resetTokenCacheService.Setup(ev => ev.CacheResetCodeCacheAsync(resetCodeCacheData)).ReturnsAsync(true);

        var result = await _userDomainService.GetResetCodeByEmail(user.Email!);

        _userRepositoryMock.Verify(ur => ur.FindByEmailAsync(user.Email!));
        _userRepositoryMock.Verify(ur => ur.GeneratePasswordResetTokenAsync(user));
        _resetTokenCacheService.Verify(ev => ev.CacheResetCodeCacheAsync(It.IsAny<ResetCodeCacheData>()));

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
        _tokenServiceMock.Setup(ts => ts.BuildToken(It.IsAny<List<Claim>>())).Returns("token");

        var result = await _userDomainService.LoginByEmailAndPasswordAsync(user.Email!, user.PasswordHash, "");

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
        _tokenServiceMock.Setup(ts => ts.BuildToken(It.IsAny<List<Claim>>())).Returns("token");

        var result = await _userDomainService.LoginByNameAndPasswordAsync(user.UserName!, user.PasswordHash, "");

        _userRepositoryMock.Verify(ur => ur.FindByNameAsync(user.UserName!));
        _userRepositoryMock.Verify(ur => ur.CheckForSignInAsync(user, user.PasswordHash!, true));

        result.Should().NotBeNull();
        result.SignInResult.Should().Be(SignInResult.Success);
        result.Token.Should().BeOfType<string>();
    }

    [Fact]
    public async Task ResetPasswordByEmailResetCodeAsync_Should_ReturnSuccess()
    {
        var user = _context.Users.First();

        var resetCode = "123";
        var newPassword = "14567";

        var resetCodeCacheData = new Domain.Data.ResetCodeCacheData(user.Id, resetCode);

        _resetTokenCacheService.Setup(ev => ev.GetResetCodeCacheAsync(It.IsAny<UserId>())).ReturnsAsync(resetCodeCacheData);

        var result = await _userDomainService.ResetPasswordByResetCodeAsync(user, resetCode, newPassword);

        _resetTokenCacheService.Verify(ev => ev.GetResetCodeCacheAsync(user.Id));

        result.Should().Be(IdentityResult.Success);
    }
}
