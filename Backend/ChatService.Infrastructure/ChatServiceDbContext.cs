using ChatService.Domain.Entities;
using Infrastructure.EFCore;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatService.Infrastructure;

public class ChatServiceDbContext : BaseDbContext
{
    public ChatServiceDbContext(DbContextOptions<ChatServiceDbContext> options, IMediator mediator)
        : base(options, mediator)
    {
    }

    public DbSet<GroupConversation> GroupConversations { get; private set; }
    public DbSet<GroupMessage> GroupMessages { get; private set; }
    public DbSet<PersonalConversation> PersonalConversations { get; private set; }
    public DbSet<PersonalMessage> PersonalMessages { get; private set; }
    public DbSet<ReplyMessage> ReplyMessages { get; private set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
