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
        }
    }
}