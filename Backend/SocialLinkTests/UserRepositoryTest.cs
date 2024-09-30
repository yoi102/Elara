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
                (userStoreMock.Object, null, null, null, null, null, null, null, null);

        }
        [Fact]
        public async Task AccessFailedAsyncTest()
        {
            //这种直接调用userManager 的，测试似乎没意义
            var userRepository = new UserRepository(_mockUserManager.Object);
            var context = await GetDataBaseContext();
            var user = context.Users.FirstOrDefault();
            user.Should().NotBeNull();

            _mockUserManager.Setup(i => i.AccessFailedAsync(user))
                              .Callback(() => user!.AccessFailedCount++);

            await userRepository.AccessFailedAsync(user!);

            Assert.Equal(1, user!.AccessFailedCount);
        }




        [Fact]
        public async Task MockDataBaseContextTest()
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