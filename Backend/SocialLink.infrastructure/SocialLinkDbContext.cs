using Infrastructure.EFCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialLink.Domain.Entities;

namespace SocialLink.infrastructure
{
    public class SocialLinkDbContext : BaseDbContext
    {
        public SocialLinkDbContext(DbContextOptions<SocialLinkDbContext> options, IMediator mediator) : base(options, mediator)
        {
        }

        public DbSet<User> Users { get; private set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
            modelBuilder.EnableSoftDeletionGlobalFilter();


            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserName)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();


            modelBuilder.Entity<User>()
                            .Property(u => u.Id)
                            .HasConversion(
           id => id.Value,  // 将 UserId 转换为 int 存储到数据库
           value => new UserId(value)  // 从数据库读取 int 并转换为 UserId
       );
        }
    }
}