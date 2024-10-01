using FluentAssertions;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using SocialLink.Domain.Entities;
using SocialLink.infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLinkTests
{
    public class UserRepositoryTest
    {
        private Mock<IdUserManager> _mockUserManager;

        public UserRepositoryTest()
        {
            var userStoreMock = new Mock<IUserStore<User>>();

            _mockUserManager = new Mock<IdUserManager>
                (userStoreMock.Object, null!, null!, null!, null!, null!, null!, null!, null!);

        }
        [Fact]
        public async Task AccessFailedAsync_Should_Call_UserManager_AccessFailedAsync()
        {
            //意味ある？
            var userRepository = new UserRepository(_mockUserManager.Object);
            var context = await GetDataBaseContext();
            var user = context.Users.First();
            user.Should().NotBeNull();

            _mockUserManager.Setup(i => i.AccessFailedAsync(user))
                              .Callback(() => user.AccessFailedCount++);
            var before = user.AccessFailedCount;
            await userRepository.AccessFailedAsync(user!);
            var after = user.AccessFailedCount;
            Assert.Equal(after, before + 1);
            _mockUserManager.Verify(um => um.AccessFailedAsync(user), Times.Once);

        }

        [Fact]
        public async Task ChangePasswordAsync_Should_Return_SucceededResult()
        {
            // Arrange

            var userRepository = new UserRepository(_mockUserManager.Object);
            var context = await GetDataBaseContext();
            var user = context.Users.First();

            _mockUserManager.Setup(um => um.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(user);


            var token = "reset-token";
            var password = "123123";

            _mockUserManager.Setup(um => um.GeneratePasswordResetTokenAsync(user))
                           .ReturnsAsync(token);

            _mockUserManager.Setup(um => um.ResetPasswordAsync(user, token, password))
                           .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await userRepository.ChangePasswordAsync(user.Id, password);

            // Assert
            _mockUserManager.Verify(um => um.FindByIdAsync(user.Id.ToString()));
            _mockUserManager.Verify(um => um.GeneratePasswordResetTokenAsync(user));
            _mockUserManager.Verify(um => um.ResetPasswordAsync(user, token, password));
            result.Succeeded.Should().BeTrue();


        }



        [Fact]
        public async Task GetDataBaseContext_Should_Contain_10_Users()
        {
            var context = await GetDataBaseContext();

            context.Should().NotBeNull();
            context.Users.Count().Should().Be(10);
        }

        private async Task<SocialLinkDbContext> GetDataBaseContext()
        {
            var mediatorMock = new Mock<IMediator>();

            var options = new DbContextOptionsBuilder<SocialLinkDbContext>()
                                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                                    .Options;
            var dbContext = new SocialLinkDbContext(options, mediatorMock.Object);
            await dbContext.Database.EnsureCreatedAsync();
            if (await dbContext.Users.CountAsync() <= 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    dbContext.Users.Add(new SocialLink.Domain.Entities.User(i.ToString(), $"{i}123@1mail.com"));
                }

                await dbContext.SaveChangesAsync();
            }

            return dbContext;
        }
    }
}