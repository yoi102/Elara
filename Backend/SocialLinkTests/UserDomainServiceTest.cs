using FluentAssertions;
using JWT;
using Microsoft.Extensions.Options;
using Moq;
using SocialLink.Domain.Entities;
using SocialLink.Domain.Interfaces;
using SocialLink.Domain.Services;
using SocialLink.infrastructure;

namespace SocialLinkTests
{
    public class UserDomainServiceTest
    {
        private Mock<IUserRepository> userRepositoryMock;
        private Mock<ITokenService> tokenServiceMock;
        private Mock<IOptions<JWTOptions>> optJWTMock;
        private Mock<IEmailResetCodeValidator> emailResetCodeValidatorMock;
        private UserDomainService userDomainService;

        public UserDomainServiceTest()
        {
            userRepositoryMock = new Mock<IUserRepository>();
            tokenServiceMock = new Mock<ITokenService>();
            optJWTMock = new Mock<IOptions<JWTOptions>>();
            emailResetCodeValidatorMock = new Mock<IEmailResetCodeValidator>();
            userDomainService = new UserDomainService(userRepositoryMock.Object,
                                              tokenServiceMock.Object,
                                                 optJWTMock.Object,
                                      emailResetCodeValidatorMock.Object);
        }



        [Fact]
        public async Task GetEmailResetCode_Should_ReturnSuccess()
        {
            var user = new User("name", "email@123.com");
            userRepositoryMock.Setup(ur => ur.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
            emailResetCodeValidatorMock.Setup(ev => ev.StashEmailResetCode(It.IsAny<string>(), It.IsAny<string>()));

            var result = await userDomainService.GetEmailResetCode(user.Email!);

            userRepositoryMock.Verify(ur => ur.FindByEmailAsync(user.Email!));
            emailResetCodeValidatorMock.Verify(ev => ev.StashEmailResetCode(user.Email!, It.IsAny<string>()));

            result.Should().NotBeNull();
            result.IdentityResult.Succeeded.Should().BeTrue();
            result.Email.Should().Be(user.Email!);
            result.HtmlMessage.Should().BeOfType<string>();
        }
    }
}