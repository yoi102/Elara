using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
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

        private async Task<SocialLinkDbContext> GetDataBaseContext()
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(l => l.DispatchDomainEventsAsync(null!)).Returns(Task.FromResult(true));

            var options = new DbContextOptionsBuilder<SocialLinkDbContext>()
                                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                                    .Options;
            var dbContext = new SocialLinkDbContext(options, mediatorMock.Object);
            await dbContext.Database.EnsureCreatedAsync();
            if (await dbContext.Users.CountAsync() <= 0)
            {




            }


            return dbContext;
        }



        [Fact]
        public void Test1()
        {

        }
    }
}
