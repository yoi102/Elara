using DomainCommons.EntityStronglyIds;
using EventBus;
using FluentAssertions;
using IdentityService.Domain.Entities;
using IdentityService.Domain.Interfaces;
using IdentityService.Domain.Results;
using IdentityService.Infrastructure;
using IdentityService.WebAPI.Controllers.User;
using IdentityService.WebAPI.Controllers.User.Request;
using IdentityService.WebAPI.Controllers.User.Response;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using System.Security.Claims;

namespace IdentityService.Tests;
//由于代码重构、后续需待重新审视
public class UserControllerTest
{
    private readonly IdentityDbContext _context;
    private readonly Mock<IEmailSender> emailSenderMock;
    private readonly Mock<ILogger<UserController>> loggerMock;
    private readonly Mock<IEventBus> eventBusMock;
    private readonly UserController userController;
    private readonly Mock<IUserDomainService> userDomainServiceMock;
    private readonly Mock<IUserRepository> userRepositoryMock;

    public UserControllerTest()
    {
        userDomainServiceMock = new Mock<IUserDomainService>();
        userRepositoryMock = new Mock<IUserRepository>();
        emailSenderMock = new Mock<IEmailSender>();
        loggerMock = new Mock<ILogger<UserController>>();
        eventBusMock = new Mock<IEventBus>();
        userController = new UserController(loggerMock.Object, userDomainServiceMock.Object,
                                   userRepositoryMock.Object, emailSenderMock.Object, eventBusMock.Object);
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
    public async Task Delete_ReturnsBadRequest_WhenUpdateFailed()
    {
        var userId = UserId.New();
        var user = new ClaimsPrincipal(new ClaimsIdentity(
        [
        new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        ]));

        userController.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        userRepositoryMock.Setup(repo => repo.RemoveUserAsync(It.IsAny<UserId>()))
                             .ReturnsAsync(IdentityResult.Failed())
                             .Verifiable();

        var result = await userController.Delete();

        var badRequestResult = Assert.IsType<BadRequestResult>(result);
        badRequestResult.StatusCode.Should().Be(400);

        userRepositoryMock.Verify(repo => repo.RemoveUserAsync(userId), Times.Once);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenUserIdIsNullOrEmpty()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(
        [
        new Claim(ClaimTypes.NameIdentifier, "")
        ]));

        userController.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        var result = await userController.Delete();

        var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
        notFoundObjectResult.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task Delete_ReturnsOk_WhenUserIsFound()
    {
        var userId = UserId.New();
        var user = new ClaimsPrincipal(new ClaimsIdentity(
        [
        new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        ]));

        userController.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        userRepositoryMock.Setup(repo => repo.RemoveUserAsync(It.IsAny<UserId>()))
                             .ReturnsAsync(IdentityResult.Success)
                             .Verifiable();

        var result = await userController.Delete();

        var okResult = Assert.IsType<OkResult>(result);
        okResult.StatusCode.Should().Be(200);

        userRepositoryMock.Verify(repo => repo.RemoveUserAsync(It.Is<UserId>(id => id == UserId.Parse(userId.ToString()))), Times.Once);
    }

    [Fact]
    public async Task GetEmailResetCode_ReturnsNotFound_WhenUserIdIsNullOrEmpty()
    {
        var email = "123@123.com";
        GetEmailResetCodeResult getEmailResetCodeResult =
            new GetEmailResetCodeResult(IdentityResult.Failed(), email, "Subject", "Message");

        userDomainServiceMock.Setup(ud => ud.GetResetCodeByEmail(It.IsAny<string>())).ReturnsAsync(getEmailResetCodeResult);

        var result = await userController.GetResetCodeByEmail(email);

        userDomainServiceMock.Verify(ud => ud.GetResetCodeByEmail(email));
        var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
        notFoundObjectResult.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task GetEmailResetCode_ReturnsOk_WhenUserIsFound()
    {
        var email = "123@123.com";
        GetEmailResetCodeResult getEmailResetCodeResult =
            new GetEmailResetCodeResult(IdentityResult.Success, email, "Subject", "Message");

        userDomainServiceMock.Setup(ud => ud.GetResetCodeByEmail(It.IsAny<string>())).ReturnsAsync(getEmailResetCodeResult);

        var result = await userController.GetResetCodeByEmail(email);

        userDomainServiceMock.Verify(ud => ud.GetResetCodeByEmail(email));

        var okResult = Assert.IsType<OkObjectResult>(result);
        okResult.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task GetUserInfo_ReturnsNotFound_WhenUserIdIsNullOrEmpty()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(
    [
                           new Claim(ClaimTypes.NameIdentifier, "")
                     ]));

        userController.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        var result = await userController.GetUserInfo();

        var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        notFoundObjectResult.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task GetUserInfo_ReturnsNotFound_WhenUserIsNotFound()
    {
        var userId = UserId.New();
        var user = new ClaimsPrincipal(new ClaimsIdentity(
        [
        new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        ]));

        userController.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        userRepositoryMock.Setup(repo => repo.FindByIdAsync(It.IsAny<UserId>()))
                             .ReturnsAsync((User)null!);

        var result = await userController.GetUserInfo();

        userRepositoryMock.Verify(repo => repo.FindByIdAsync(userId), Times.Once);
        var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        notFoundObjectResult.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task GetUserInfo_ReturnsOk_WhenUserIsFound()
    {
        var userMock = new User("user", "123123@qwe.com");
        var user = new ClaimsPrincipal(new ClaimsIdentity(
        [
        new Claim(ClaimTypes.NameIdentifier, userMock.Id.ToString())
        ]));

        userController.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        userRepositoryMock.Setup(repo => repo.FindByIdAsync(It.IsAny<UserId>()))
                             .ReturnsAsync(userMock);

        var result = await userController.GetUserInfo();

        userRepositoryMock.Verify(repo => repo.FindByIdAsync(It.Is<UserId>(id => id == userMock.Id)), Times.Once);
        result.Result.Should().BeNull();
        var userInfo = Assert.IsType<GetUserInfoResponse>(result.Value);
        userInfo.Id.Should().Be(userMock.Id);
        userInfo.Name.Should().Be(userMock.UserName);
    }

    [Fact]
    public async Task LoginByEmailAndPassword_ReturnsLocked_WhenAccountIsLocked()
    {
        // Arrange
        var loginRequest = new LoginByEmailAndPasswordRequest(

             "test@example.com",
             "Password123",
             ""
         );

        var loginResult = new LoginResult
        {
            SignInResult = Microsoft.AspNetCore.Identity.SignInResult.LockedOut,
            Token = "some-jwt-token"
        };

        userDomainServiceMock.Setup(service => service.LoginByEmailAndPasswordAsync(It.IsAny<string>(), It.IsAny<string>(),It.IsAny<string>()))
                                            .ReturnsAsync(loginResult);

        // Act
        var result = await userController.LoginByEmailAndPassword(loginRequest);

        // Assert
        userDomainServiceMock.Verify(service => service.LoginByEmailAndPasswordAsync(loginRequest.Email, loginRequest.Password, ""));
    }

    [Fact]
    public async Task LoginByEmailAndPassword_ReturnsToken_WhenLoginIsSuccessful()
    {
        // Arrange
        var loginRequest = new LoginByEmailAndPasswordRequest(

            "test@example.com",
            "Password123",
            ""
        );
        var loginResult = new LoginResult
        {
            SignInResult = Microsoft.AspNetCore.Identity.SignInResult.Success,
            Token = "some-jwt-token"
        };

        userDomainServiceMock.Setup(service => service.LoginByEmailAndPasswordAsync(It.IsAny<string>(), It.IsAny<string>(),It.IsAny<string>()))
                                                                                              .ReturnsAsync(loginResult);

        // Act
        var result = await userController.LoginByEmailAndPassword(loginRequest);

        // Assert
        userDomainServiceMock.Verify(service => service.LoginByEmailAndPasswordAsync(loginRequest.Email, loginRequest.Password, ""));
    }

    [Fact]
    public async Task LoginByEmailAndPassword_ReturnsUnauthorized_WhenLoginFails()
    {
        // Arrange
        var loginRequest = new LoginByEmailAndPasswordRequest(

           "test@example.com",
           "Password123",
           ""
       );

        var loginResult = new LoginResult
        {
            SignInResult = Microsoft.AspNetCore.Identity.SignInResult.Failed,
            Token = "some-jwt-token"
        };

        userDomainServiceMock.Setup(service => service.LoginByEmailAndPasswordAsync(It.IsAny<string>(), It.IsAny<string>(), ""))
            .ReturnsAsync(loginResult);

        // Act
        var result = await userController.LoginByEmailAndPassword(loginRequest);

        // Assert
        userDomainServiceMock.Verify(service => service.LoginByEmailAndPasswordAsync(loginRequest.Email, loginRequest.Password, ""));
    }

    [Fact]
    public async Task LoginByNameAndPassword_ReturnsLocked_WhenAccountIsLocked()
    {
        // Arrange
        var loginRequest = new LoginByNameAndPasswordRequest(

            "name",
            "Password123",
            ""

        );

        var loginResult = new LoginResult
        {
            SignInResult = Microsoft.AspNetCore.Identity.SignInResult.LockedOut,
            Token = "some-jwt-token"
        };

        userDomainServiceMock.Setup(service => service.LoginByNameAndPasswordAsync(It.IsAny<string>(), It.IsAny<string>(), ""))
                                                                                              .ReturnsAsync(loginResult);

        // Act
        var result = await userController.LoginByNameAndPassword(loginRequest);

        // Assert
        userDomainServiceMock.Verify(service => service.LoginByNameAndPasswordAsync(loginRequest.Name, loginRequest.Password, ""));
    }

    [Fact]
    public async Task LoginByNameAndPassword_ReturnsToken_WhenLoginIsSuccessful()
    {
        // Arrange
        var loginRequest = new LoginByNameAndPasswordRequest(

            "name",
            "Password123",
            ""
        );
        var loginResult = new LoginResult
        {
            SignInResult = Microsoft.AspNetCore.Identity.SignInResult.Success,
            Token = "some-jwt-token"
        };

        userDomainServiceMock.Setup(service => service.LoginByNameAndPasswordAsync(It.IsAny<string>(), It.IsAny<string>(), ""))
                                                                                              .ReturnsAsync(loginResult);

        // Act
        var result = await userController.LoginByNameAndPassword(loginRequest);

        // Assert
        userDomainServiceMock.Verify(service => service.LoginByNameAndPasswordAsync(loginRequest.Name, loginRequest.Password, ""));
    }

    [Fact]
    public async Task LoginByNameAndPassword_ReturnsUnauthorized_WhenLoginFails()
    {
        // Arrange
        var loginRequest = new LoginByNameAndPasswordRequest(

            "name",
            "Password123",
            ""
        );

        var loginResult = new LoginResult
        {
            SignInResult = Microsoft.AspNetCore.Identity.SignInResult.Failed,
            Token = "some-jwt-token"
        };

        userDomainServiceMock.Setup(service => service.LoginByNameAndPasswordAsync(It.IsAny<string>(), It.IsAny<string>(), ""))
                                                                                              .ReturnsAsync(loginResult);

        // Act
        var result = await userController.LoginByNameAndPassword(loginRequest);

        // Assert
        userDomainServiceMock.Verify(service => service.LoginByNameAndPasswordAsync(loginRequest.Name, loginRequest.Password, ""));
    }

    [Fact]
    public async Task ResetPassword_ReturnsBadRequest_WhenResetPasswordFails()
    {
        // Arrange
        var userId = UserId.New();
        userRepositoryMock.Setup(repo => repo.ResetPasswordByOldPasswordAsync(It.IsAny<UserId>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Password too weak" }));

        var user = new ClaimsPrincipal(new ClaimsIdentity(
        [
        new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        ]));

        userController.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        // Act
        var result = await userController.ResetPassword("oldPassword", "weak");

        // Assert
        userRepositoryMock.Verify(repo => repo.ResetPasswordByOldPasswordAsync(userId, "oldPassword", "weak"));
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        badRequestResult.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task ResetPassword_ReturnsNotFound_WhenUserIsNotLoggedIn()
    {
        string oldPassword = "oldPassword";
        string newPassword = "newPassword";
        var user = new ClaimsPrincipal(new ClaimsIdentity(
        [
        new Claim(ClaimTypes.NameIdentifier, string.Empty)
        ]));

        userController.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        var result = await userController.ResetPassword(oldPassword, newPassword);

        var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
        notFoundObjectResult.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task ResetPassword_ReturnsOk_WhenResetPasswordSucceeds()
    {
        // Arrange
        var userId = UserId.New();
        userRepositoryMock.Setup(repo => repo.ResetPasswordByOldPasswordAsync(It.IsAny<UserId>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        var user = new ClaimsPrincipal(new ClaimsIdentity(
        [
        new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        ]));

        userController.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        // Act
        var result = await userController.ResetPassword("oldPassword", "NewStrongPassword123");

        // Assert
        userRepositoryMock.Verify(repo => repo.ResetPasswordByOldPasswordAsync(userId, "oldPassword", "NewStrongPassword123"));
        var okResult = Assert.IsType<OkObjectResult>(result);
        okResult.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task ResetPasswordWithEmailCode_ReturnsBadRequest_WhenPasswordResetFails()
    {
        // Arrange

        var user = _context.Users.First();
        var resetCode = "123456";
        var newPassword = "NewStrongPassword123";
        var email = user.Email!;
        var request = new ResetPasswordRequest()
        {
            ResetCode = resetCode,
            Email = email,
            NewPassword = newPassword
        };
        userRepositoryMock.Setup(repo => repo.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(user);
        userDomainServiceMock.Setup(service => service.ResetPasswordByResetCodeAsync(user, resetCode, newPassword))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Invalid code" }));

        // Act
        var result = await userController.ResetPasswordWithEmailCode(request);

        // Assert
        userRepositoryMock.Verify(repo => repo.FindByEmailAsync(request.Email));
        userDomainServiceMock.Verify(service => service.ResetPasswordByResetCodeAsync(user, resetCode, newPassword));
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        badRequestResult.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task ResetPasswordWithEmailCode_ReturnsNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        ResetPasswordRequest request = new ResetPasswordRequest()
        {
            Email = "123@123.com",
            NewPassword = "NewStrongPassword123",
            ResetCode = "123456"
        };
        userRepositoryMock.Setup(repo => repo.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((User)null!);

        // Act
        var result = await userController.ResetPasswordWithEmailCode(request);

        // Assert
        userRepositoryMock.Verify(repo => repo.FindByEmailAsync(request.Email));
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        notFoundResult.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task ResetPasswordWithEmailCode_ReturnsOk_WhenPasswordResetSucceeds()
    {
        var user = _context.Users.First();
        // Arrange
        var request = new ResetPasswordRequest()
        {
            Email = user.Email!,
            NewPassword = "NewStrongPassword123",
            ResetCode = "123456"
        };

        userRepositoryMock.Setup(repo => repo.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(user);
        userDomainServiceMock.Setup(service => service.ResetPasswordByResetCodeAsync(user, request.ResetCode, request.NewPassword))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await userController.ResetPasswordWithEmailCode(request);

        // Assert
        userRepositoryMock.Verify(repo => repo.FindByEmailAsync(request.Email));
        userDomainServiceMock.Verify(service => service.ResetPasswordByResetCodeAsync(user, request.ResetCode, request.NewPassword));
        var okResult = Assert.IsType<OkObjectResult>(result);
        okResult.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task SignUp_ReturnsBadRequest_WhenSignUpFails()
    {
        // Arrange
        var request = new SignUpRequest("testName", "2test@example.com", "Password123");

        userRepositoryMock.Setup(repo => repo.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((User)null!);
        userRepositoryMock.Setup(repo => repo.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((User)null!);

        userRepositoryMock.Setup(repo => repo.SignUpAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(new SignUpResult
            {
                User = null,
                IdentityResult = IdentityResult.Failed(new IdentityError { Description = "Weak password" })
            });

        // Act
        var result = await userController.SignUp(request);

        // Assert
        userRepositoryMock.Verify(repo => repo.FindByNameAsync(request.Name));
        userRepositoryMock.Verify(repo => repo.FindByEmailAsync(request.Email));
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        badRequestResult.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task SignUp_ReturnsConflict_WhenEmailAlreadyExists()
    {
        // Arrange
        var user = new User("userName", "test@example.com");
        var request = new SignUpRequest("testName", user.Email!, "Password123");

        // 模拟邮箱已存在
        userRepositoryMock.Setup(repo => repo.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((User)null!);
        userRepositoryMock.Setup(repo => repo.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(user); // 邮箱已存在

        // Act
        var result = await userController.SignUp(request);

        // Assert
        userRepositoryMock.Verify(repo => repo.FindByNameAsync(request.Name));
        userRepositoryMock.Verify(repo => repo.FindByEmailAsync(request.Email));

        var conflictResult = Assert.IsType<ConflictObjectResult>(result);
        conflictResult.StatusCode.Should().Be(409);
    }

    [Fact]
    public async Task SignUp_ReturnsConflict_WhenUsernameAlreadyExists()
    {
        // Arrange
        var user = new User("userName", "test@example.com");
        var request = new SignUpRequest(user.UserName!, user.Email!, "Password123");

        userRepositoryMock.Setup(repo => repo.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

        // Act
        var result = await userController.SignUp(request);

        // Assert
        userRepositoryMock.Verify(repo => repo.FindByNameAsync(request.Name));

        var conflictResult = Assert.IsType<ConflictObjectResult>(result);
        conflictResult.StatusCode.Should().Be(409);
    }

    [Fact]
    public async Task SignUp_ReturnsCreated_WhenSignUpSucceeds()
    {
        // Arrange
        var request = new SignUpRequest("testName", "2test@example.com", "Password123");

        userRepositoryMock.Setup(repo => repo.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((User)null!);
        userRepositoryMock.Setup(repo => repo.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((User)null!);

        var userId = Guid.NewGuid();
        userRepositoryMock.Setup(repo => repo.SignUpAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(new SignUpResult
            {
                IdentityResult = IdentityResult.Success,
                User = new User("userName", "test@example.com")
            });

        // Act
        var result = await userController.SignUp(request);

        // Assert
        userRepositoryMock.Verify(repo => repo.FindByNameAsync(request.Name));
        userRepositoryMock.Verify(repo => repo.FindByEmailAsync(request.Email));
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        createdResult.StatusCode.Should().Be(201);
    }
}
