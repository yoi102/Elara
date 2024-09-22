using FluentAssertions;
using IdentityService.Domain.Entities;
using IdentityService.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace IdentityService.Tests;
//由于代码重构、后续需待重新审视

public class UserRepositoryTest
{
    private readonly IdentityDbContext _context;
    private readonly Mock<IdUserManager> _mockUserManager;
    private readonly UserRepository _userRepository;
    private readonly Mock<IUserLoginStore<User>> _userStoreMock;

    public UserRepositoryTest()
    {
        _userStoreMock = new Mock<IUserLoginStore<User>>();

        _mockUserManager = new Mock<IdUserManager>
            (_userStoreMock.Object, null!, null!, null!, null!, null!, null!, null!, null!);
        _userRepository = new UserRepository(_mockUserManager.Object);

        _context = GetDataBaseContext().Result;
    }

    [Fact]
    public async Task AccessFailedAsync_Should_Call_UserManager_AccessFailedAsync()
    {
        var user = _context.Users.First();
        user.Should().NotBeNull();

        _mockUserManager.Setup(i => i.AccessFailedAsync(user))
                          .Callback(() => user.AccessFailedCount++);
        var before = user.AccessFailedCount;
        await _userRepository.AccessFailedAsync(user!);
        var after = user.AccessFailedCount;
        Assert.Equal(after, before + 1);
        _mockUserManager.Verify(um => um.AccessFailedAsync(user), Times.Once);
    }

    [Fact]
    public async Task ChangePasswordAsync_Should_Return_SucceededResult()
    {
        // Arrange

        var user = _context.Users.First();

        var token = "reset-resetToken";
        var password = "123123";

        _mockUserManager.Setup(um => um.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(user);

        _mockUserManager.Setup(um => um.GeneratePasswordResetTokenAsync(It.IsAny<User>()))
                       .ReturnsAsync(token);

        _mockUserManager.Setup(um => um.ResetPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                       .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _userRepository.ChangePasswordAsync(user.Id, password);

        // Assert
        _mockUserManager.Verify(um => um.FindByIdAsync(user.Id.ToString()));
        _mockUserManager.Verify(um => um.GeneratePasswordResetTokenAsync(user));
        _mockUserManager.Verify(um => um.ResetPasswordAsync(user, token, password));
        result.Succeeded.Should().BeTrue();
    }

    [Fact]
    public async Task CheckForSignInAsync_Should_Return_Failed()
    {
        var user = _context.Users.First();

        _mockUserManager.Setup(um => um.IsLockedOutAsync(It.IsAny<User>())).ReturnsAsync(false);
        _mockUserManager.Setup(um => um.CheckPasswordAsync(It.IsAny<User>(), user.PasswordHash!)).ReturnsAsync(false);
        _mockUserManager.Setup(um => um.AccessFailedAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success);

        var result = await _userRepository.CheckForSignInAsync(user, user.PasswordHash!, false);

        _mockUserManager.Verify(um => um.IsLockedOutAsync(user));
        _mockUserManager.Verify(um => um.CheckPasswordAsync(user, user.PasswordHash!));
        _mockUserManager.Verify(um => um.AccessFailedAsync(user));

        result.Should().Be(SignInResult.Failed);
    }

    [Fact]
    public async Task CheckForSignInAsync_Should_Return_LockedOut()
    {
        var user = _context.Users.First();

        _mockUserManager.Setup(um => um.IsLockedOutAsync(It.IsAny<User>())).ReturnsAsync(true);

        var result = await _userRepository.CheckForSignInAsync(user, user.PasswordHash!, true);

        _mockUserManager.Verify(um => um.IsLockedOutAsync(user));

        result.Should().Be(SignInResult.LockedOut);
    }

    [Fact]
    public async Task CheckForSignInAsync_Should_Return_SucceededResult()
    {
        var user = _context.Users.First();

        _mockUserManager.Setup(um => um.IsLockedOutAsync(It.IsAny<User>())).ReturnsAsync(false);
        _mockUserManager.Setup(um => um.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(true);

        var result = await _userRepository.CheckForSignInAsync(user, user.PasswordHash!, true);

        _mockUserManager.Verify(um => um.IsLockedOutAsync(user));
        _mockUserManager.Verify(um => um.CheckPasswordAsync(user, user.PasswordHash!));

        result.Should().Be(SignInResult.Success);
    }

    [Fact]
    public async Task FindByEmailAsync_Should_Return_User()
    {
        var user = _context.Users.First();
        var email = user.Email!;
        _mockUserManager.Setup(um => um.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);

        var result = await _userRepository.FindByEmailAsync(email);

        _mockUserManager.Verify(um => um.FindByEmailAsync(email));

        result.Should().BeOfType<User>();
    }

    [Fact]
    public async Task FindByIdAsync_Should_Return_User()
    {
        var user = _context.Users.First();
        var id = user.Id!;
        _mockUserManager.Setup(um => um.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(user);

        var result = await _userRepository.FindByIdAsync(id);

        _mockUserManager.Verify(um => um.FindByIdAsync(id.ToString()));

        result.Should().BeOfType<User>();
    }

    [Fact]
    public async Task FindByNameAsync_Should_Return_User()
    {
        var user = _context.Users.First();
        var userName = user.UserName!;
        _mockUserManager.Setup(um => um.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(user);

        var result = await _userRepository.FindByNameAsync(userName);

        _mockUserManager.Verify(um => um.FindByNameAsync(userName));

        result.Should().BeOfType<User>();
    }

    [Fact]
    public async Task RemoveUserAsync_Should_Return_Success()
    {
        var user = _context.Users.First();
        var userName = user.UserName!;
        var mockUserLoginStore = new Mock<IUserLoginStore<User>>();
        var logins = new List<UserLoginInfo> { new UserLoginInfo("provider", "key", "displayName") };

        _userStoreMock.Setup(uls => uls.GetLoginsAsync(It.IsAny<User>(), default)).ReturnsAsync(logins);
        _userStoreMock.Setup(uls => uls.RemoveLoginAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>(), default))
                          .Returns(Task.CompletedTask);
        _mockUserManager.Setup(um => um.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(user);
        _mockUserManager.Setup(um => um.UpdateAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success);

        var result = await _userRepository.RemoveUserAsync(user.Id);

        _userStoreMock.Verify(uls => uls.GetLoginsAsync(user, default), Times.Once);
        _userStoreMock.Verify(uls => uls.RemoveLoginAsync(user, "provider", "key", default), Times.Once);

        _mockUserManager.Verify(um => um.FindByIdAsync(user.Id.ToString()));
        _mockUserManager.Verify(um => um.UpdateAsync(user));

        user.IsDeleted.Should().BeTrue();
        result.Should().Be(IdentityResult.Success);
    }

    [Fact]
    public async Task ResetPasswordByEmailAsync_Should_Return_Success()
    {
        var user = _context.Users.First();
        var newPassword = "new-password";
        var resetToken = "reset-resetToken";

        _mockUserManager.Setup(um => um.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(false);
        _mockUserManager.Setup(um => um.ResetPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

        var result = await _userRepository.ResetPasswordByResetTokenAsync(user, newPassword, resetToken);

        _mockUserManager.Verify(um => um.CheckPasswordAsync(user, newPassword));
        _mockUserManager.Verify(um => um.ResetPasswordAsync(user, resetToken, newPassword));
        result.Should().Be(IdentityResult.Success);
    }

    [Fact]
    public async Task ResetPasswordByIdAsync_Should_Return_Success()
    {
        var user = _context.Users.First();
        var oldPassword = "old-password";
        var newPassword = "new-password";
        var resetToken = "resetToken";

        _mockUserManager.Setup(um => um.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(user);
        _mockUserManager.Setup(um => um.CheckPasswordAsync(It.IsAny<User>(), oldPassword)).ReturnsAsync(true);
        _mockUserManager.Setup(um => um.CheckPasswordAsync(It.IsAny<User>(), newPassword)).ReturnsAsync(false);
        _mockUserManager.Setup(um => um.GeneratePasswordResetTokenAsync(It.IsAny<User>())).ReturnsAsync(resetToken);
        _mockUserManager.Setup(um => um.ResetPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

        var result = await _userRepository.ResetPasswordByOldPasswordAsync(user.Id, oldPassword, newPassword);

        _mockUserManager.Verify(um => um.FindByIdAsync(user.Id.ToString()));
        _mockUserManager.Verify(um => um.CheckPasswordAsync(user, oldPassword));
        _mockUserManager.Verify(um => um.GeneratePasswordResetTokenAsync(user));
        _mockUserManager.Verify(um => um.ResetPasswordAsync(user, resetToken, newPassword));
        result.Should().Be(IdentityResult.Success);
    }

    [Fact]
    public async Task SignUpAsync_Should_Return_Success()
    {
        var name = "123453";
        var email = "123@asd.com";
        var password = "password";
        var user = new User(name, email);

        _mockUserManager.Setup(um => um.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
        _mockUserManager.Setup(um => um.FindByNameAsync(name)).ReturnsAsync(user);

        var result = await _userRepository.SignUpAsync(name, email, password);

        _mockUserManager.Verify(um => um.CreateAsync(It.IsAny<User>(), password));
        _mockUserManager.Verify(um => um.FindByNameAsync(name));

        result.IdentityResult.Should().Be(IdentityResult.Success);
        result.User.Should().BeOfType<User>();
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
}
