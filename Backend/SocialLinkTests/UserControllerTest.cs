using EventBus;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SocialLink.Domain.Entities;
using SocialLink.Domain.Interfaces;
using SocialLink.WebAPI.Controllers.User;
using System.Security.Claims;

namespace SocialLinkTests
{
    public class UserControllerTest
    {
        private Mock<IEventBus> eventBus;
        private UserController userController;
        private Mock<IUserDomainService> userDomainServiceMock;
        private Mock<IUserRepository> userRepository;
        public UserControllerTest()
        {
            userDomainServiceMock = new Mock<IUserDomainService>();
            userRepository = new Mock<IUserRepository>();
            eventBus = new Mock<IEventBus>();
            userController = new UserController(userDomainServiceMock.Object,
                                       userRepository.Object, eventBus.Object);
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

            userRepository.Setup(repo => repo.RemoveUserAsync(It.IsAny<UserId>()))
                                 .ReturnsAsync(IdentityResult.Failed())
                                 .Verifiable();

            var result = await userController.Delete();

            var badRequestResult = Assert.IsType<BadRequestResult>(result);
            badRequestResult.StatusCode.Should().Be(400);

            userRepository.Verify(repo => repo.RemoveUserAsync(It.Is<UserId>(id => id == UserId.Parse(userId.ToString()))), Times.Once);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenUserIdIsNullOrEmpty()
        {
            var userId = Guid.NewGuid();
            var user = new ClaimsPrincipal(new ClaimsIdentity(
            [
            new Claim(ClaimTypes.Name, userId.ToString())
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

            userRepository.Setup(repo => repo.RemoveUserAsync(It.IsAny<UserId>()))
                                 .ReturnsAsync(IdentityResult.Success)
                                 .Verifiable();

            var result = await userController.Delete();

            var okResult = Assert.IsType<OkResult>(result);
            okResult.StatusCode.Should().Be(200);

            userRepository.Verify(repo => repo.RemoveUserAsync(It.Is<UserId>(id => id == UserId.Parse(userId.ToString()))), Times.Once);
        }
    }
}