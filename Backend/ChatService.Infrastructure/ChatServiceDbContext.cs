using ChatService.Domain.Entities;
using Infrastructure.EFCore;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatService.Infrastructure {
    public class ChatServiceDbContext : BaseDbContext {
        public DbSet<GroupConversation> GroupConversations { get; private set; }
        public DbSet<GroupMessage> GroupMessages { get; private set; }
        public DbSet<GroupMessage> PersonalConversations { get; private set; }
        public DbSet<GroupMessage> PersonalMessages { get; private set; }
        public DbSet<GroupMessage> ReplyMessages { get; private set; }

        public ChatServiceDbContext(DbContextOptions<ChatServiceDbContext> options, IMediator mediator)
            : base(options, mediator)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
