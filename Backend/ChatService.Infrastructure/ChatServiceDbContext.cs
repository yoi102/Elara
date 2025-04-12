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

    public DbSet<Conversation> Conversations { get; private set; }
    public DbSet<Participant> Participants { get; private set; }
    public DbSet<Message> Messages { get; private set; }
    public DbSet<ReplyMessage> ReplyMessages { get; private set; }
    public DbSet<MessageAttachment> MessageAttachments { get; private set; }
    public DbSet<UserUnreadMessage> UserUnreadMessages { get; private set; }
    public DbSet<ConversationRequest> ConversationRequests { get; private set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
