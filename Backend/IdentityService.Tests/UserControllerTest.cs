using EventBus;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Moq;
using IdentityService.Domain.Entities;
using IdentityService.Domain.Interfaces;
using IdentityService.Domain.Results;
using IdentityService.WebAPI.Controllers.User;
using IdentityService.WebAPI.Controllers.User.Request;
using IdentityService.WebAPI.Controllers.User.Response;
using IdentityService.WebAPI.Events;
using System.Net;
using System.Security.Claims;
using DomainCommons.EntityStronglyIds;

namespace IdentityService.Tests
{
    public class UserControllerTest
    {
        private Mock<IEventBus> eventBusMock;
        private UserController userController;
        private Mock<IUserDomainService> userDomainServiceMock;
        private Mock<IUserRepository> userRepositoryMock;

        public UserControllerTest()
        {
            userDomainServiceMock = new Mock<IUserDomainService>();
            userRepositoryMock = new Mock<IUserRepository>();
            eventBusMock = new Mock<IEventBus>();
            userController = new UserController(userDomainServiceMock.Object,
                                       userRepositoryMock.Object, eventBusMock.Object);
        }

        [Fact]
        public async Task Delete_ReturnsBadRequest_WhenUpdateFailed()
        {
            var userId = Guid.NewGuid();
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

            userRepositoryMock.Verify(repo => repo.RemoveUserAsync(It.Is<UserId>(id => id.Value == userId)), Times.Once);
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
            var userId = Guid.NewGuid();
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
            ResetPasswordByEmailResetCodeEvent resetPasswordByEmailResetCodeEvent =
                new ResetPasswordByEmailResetCodeEvent(getEmailResetCodeResult.Email, getEmailResetCodeResult.Subject, getEmailResetCodeResult.HtmlMessage);
            userDomainServiceMock.Setup(ud => ud.GetEmailResetCode(It.IsAny<string>())).ReturnsAsync(getEmailResetCodeResult);

            var result = await userController.GetEmailResetCode(email);

            userDomainServiceMock.Verify(ud => ud.GetEmailResetCode(email));
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            notFoundObjectResult.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task GetEmailResetCode_ReturnsOk_WhenUserIsFound()
        {
            var email = "123@123.com";
            GetEmailResetCodeResult getEmailResetCodeResult =
                new GetEmailResetCodeResult(IdentityResult.Success, email, "Subject", "Message");
            ResetPasswordByEmailResetCodeEvent resetPasswordByEmailResetCodeEvent =
                new ResetPasswordByEmailResetCodeEvent(getEmailResetCodeResult.Email, getEmailResetCodeResult.Subject, getEmailResetCodeResult.HtmlMessage);
            userDomainServiceMock.Setup(ud => ud.GetEmailResetCode(It.IsAny<string>())).ReturnsAsync(getEmailResetCodeResult);
            eventBusMock.Setup(eb => eb.Publish(It.IsAny<string>(), It.IsAny<ResetPasswordByEmailResetCodeEvent>()));

            var result = await userController.GetEmailResetCode(email);

            userDomainServiceMock.Verify(ud => ud.GetEmailResetCode(email));
            eventBusMock.Verify(eb => eb.Publish("UserService.User.ResetUserPasswordByEmail", resetPasswordByEmailResetCodeEvent));
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
            var userId = Guid.NewGuid();
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

            userRepositoryMock.Verify(repo => repo.FindByIdAsync(It.Is<UserId>(id => id.Value == userId)), Times.Once);
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
                 "Password123"
             );

            var loginResult = new LoginResult
            {
                SignInResult = Microsoft.AspNetCore.Identity.SignInResult.LockedOut,
                Token = "some-jwt-token"
            };

            userDomainServiceMock.Setup(service => service.LoginByEmailAndPasswordAsync(It.IsAny<string>(), It.IsAny<string>()))
                                                .ReturnsAsync(loginResult);

            // Act
            var result = await userController.LoginByEmailAndPassword(loginRequest);

            // Assert
            userDomainServiceMock.Verify(service => service.LoginByEmailAndPasswordAsync(loginRequest.Email, loginRequest.Password));
            var lockedResult = Assert.IsType<ObjectResult>(result.Result);
            lockedResult.StatusCode.Should().Be((int)HttpStatusCode.Locked);
        }

        [Fact]
        public async Task LoginByEmailAndPassword_ReturnsToken_WhenLoginIsSuccessful()
        {
            // Arrange
            var loginRequest = new LoginByEmailAndPasswordRequest(

                "test@example.com",
                "Password123"
            );
            var loginResult = new LoginResult
            {
                SignInResult = Microsoft.AspNetCore.Identity.SignInResult.Success,
                Token = "some-jwt-token"
            };

            userDomainServiceMock.Setup(service => service.LoginByEmailAndPasswordAsync(It.IsAny<string>(), It.IsAny<string>()))
                                                                                                  .ReturnsAsync(loginResult);

            // Act
            var result = await userController.LoginByEmailAndPassword(loginRequest);

            // Assert
            userDomainServiceMock.Verify(service => service.LoginByEmailAndPasswordAsync(loginRequest.Email, loginRequest.Password));
            result.Result.Should().BeNull();
            result.Value.Should().Be(loginResult.Token);
        }

        [Fact]
        public async Task LoginByEmailAndPassword_ReturnsUnauthorized_WhenLoginFails()
        {
            // Arrange
            var loginRequest = new LoginByEmailAndPasswordRequest(

               "test@example.com",
               "Password123"
           );

            var loginResult = new LoginResult
            {
                SignInResult = Microsoft.AspNetCore.Identity.SignInResult.Failed,
                Token = "some-jwt-token"
            };

            userDomainServiceMock.Setup(service => service.LoginByEmailAndPasswordAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(loginResult);

            // Act
            var result = await userController.LoginByEmailAndPassword(loginRequest);

            // Assert
            userDomainServiceMock.Verify(service => service.LoginByEmailAndPasswordAsync(loginRequest.Email, loginRequest.Password));
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result.Result);
            unauthorizedResult.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task LoginByNameAndPassword_ReturnsLocked_WhenAccountIsLocked()
        {
            // Arrange
            var loginRequest = new LoginByNameAndPasswordRequest(

                "name",
                "Password123"
            );

            var loginResult = new LoginResult
            {
                SignInResult = Microsoft.AspNetCore.Identity.SignInResult.LockedOut,
                Token = "some-jwt-token"
            };

            userDomainServiceMock.Setup(service => service.LoginByNameAndPasswordAsync(It.IsAny<string>(), It.IsAny<string>()))
                                                                                                  .ReturnsAsync(loginResult);

            // Act
            var result = await userController.LoginByNameAndPassword(loginRequest);

            // Assert
            userDomainServiceMock.Verify(service => service.LoginByNameAndPasswordAsync(loginRequest.Name, loginRequest.Password));
            var lockedResult = Assert.IsType<ObjectResult>(result.Result);
            lockedResult.StatusCode.Should().Be((int)HttpStatusCode.Locked);
        }

        [Fact]
        public async Task LoginByNameAndPassword_ReturnsToken_WhenLoginIsSuccessful()
        {
            // Arrange
            var loginRequest = new LoginByNameAndPasswordRequest(

                "name",
                "Password123"
            );
            var loginResult = new LoginResult
            {
                SignInResult = Microsoft.AspNetCore.Identity.SignInResult.Success,
                Token = "some-jwt-token"
            };

            userDomainServiceMock.Setup(service => service.LoginByNameAndPasswordAsync(It.IsAny<string>(), It.IsAny<string>()))
                                                                                                  .ReturnsAsync(loginResult);

            // Act
            var result = await userController.LoginByNameAndPassword(loginRequest);

            // Assert
            userDomainServiceMock.Verify(service => service.LoginByNameAndPasswordAsync(loginRequest.Name, loginRequest.Password));
            result.Result.Should().BeNull();
            result.Value.Should().Be(loginResult.Token);
        }

        [Fact]
        public async Task LoginByNameAndPassword_ReturnsUnauthorized_WhenLoginFails()
        {
            // Arrange
            var loginRequest = new LoginByNameAndPasswordRequest(

                "name",
                "Password123"
            );

            var loginResult = new LoginResult
            {
                SignInResult = Microsoft.AspNetCore.Identity.SignInResult.Failed,
                Token = "some-jwt-token"
            };

            userDomainServiceMock.Setup(service => service.LoginByNameAndPasswordAsync(It.IsAny<string>(), It.IsAny<string>()))
                                                                                                  .ReturnsAsync(loginResult);

            // Act
            var result = await userController.LoginByNameAndPassword(loginRequest);

            // Assert
            userDomainServiceMock.Verify(service => service.LoginByNameAndPasswordAsync(loginRequest.Name, loginRequest.Password));
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result.Result);
            unauthorizedResult.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task ResetPassword_ReturnsBadRequest_WhenResetPasswordFails()
        {
            // Arrange
            var userId = UserId.New();
            userRepositoryMock.Setup(repo => repo.ResetPasswordByIdAsync(It.IsAny<UserId>(), It.IsAny<string>()))
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
            var result = await userController.ResetPassword("weak");

            // Assert
            userRepositoryMock.Verify(repo => repo.ResetPasswordByIdAsync(userId, "weak"));
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            badRequestResult.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task ResetPassword_ReturnsNotFound_WhenUserIsNotLoggedIn()
        {
            string newPassword = "";
            var user = new ClaimsPrincipal(new ClaimsIdentity(
            [
            new Claim(ClaimTypes.NameIdentifier, string.Empty)
            ]));

            userController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            var result = await userController.ResetPassword(newPassword);

            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            notFoundObjectResult.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task ResetPassword_ReturnsOk_WhenResetPasswordSucceeds()
        {
            // Arrange
            var userId = UserId.New();
            userRepositoryMock.Setup(repo => repo.ResetPasswordByIdAsync(It.IsAny<UserId>(), It.IsAny<string>()))
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
            var result = await userController.ResetPassword("NewStrongPassword123");

            // Assert
            userRepositoryMock.Verify(repo => repo.ResetPasswordByIdAsync(userId, "NewStrongPassword123"));
            var okResult = Assert.IsType<OkObjectResult>(result);
            okResult.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task ResetPasswordWithEmailCode_ReturnsBadRequest_WhenPasswordResetFails()
        {
            // Arrange

            var request = new ResetPasswordRequest()
            {
                Email = "123@123.com",
                NewPassword = "NewStrongPassword123",
                ResetCode = "123456"
            };
            var user = new User("userName", request.Email);
            userRepositoryMock.Setup(repo => repo.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            userDomainServiceMock.Setup(service => service.ResetPasswordByEmailResetCodeAsync(It.IsAny<ResetPasswordRequest>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Invalid code" }));

            // Act
            var result = await userController.ResetPasswordWithEmailCode(request);

            // Assert
            userRepositoryMock.Verify(repo => repo.FindByEmailAsync(request.Email));
            userDomainServiceMock.Verify(service => service.ResetPasswordByEmailResetCodeAsync(request));
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
            // Arrange
            var request = new ResetPasswordRequest()
            {
                Email = "123@123.com",
                NewPassword = "NewStrongPassword123",
                ResetCode = "123456"
            };
            var user = new User("userName", request.Email);

            userRepositoryMock.Setup(repo => repo.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            userDomainServiceMock.Setup(service => service.ResetPasswordByEmailResetCodeAsync(It.IsAny<ResetPasswordRequest>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await userController.ResetPasswordWithEmailCode(request);

            // Assert
            userRepositoryMock.Verify(repo => repo.FindByEmailAsync(request.Email));
            userDomainServiceMock.Verify(service => service.ResetPasswordByEmailResetCodeAsync(request));
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
            eventBusMock.Setup(eb => eb.Publish(It.IsAny<string>(), It.IsAny<UserCreatedEvent>()));

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
            eventBusMock.Verify(bus => bus.Publish("UserService.User.Created", It.IsAny<UserCreatedEvent>()), Times.Once);
        }
    }
}