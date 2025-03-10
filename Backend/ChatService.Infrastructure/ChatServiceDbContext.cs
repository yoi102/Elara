﻿using ChatService.Domain.Entities;
using DomainCommons.EntityStronglyIds;
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

    public DbSet<UserConversation> UserConversations { get; private set; }
    public DbSet<Conversation> GroupConversations { get; private set; }
    public DbSet<Participant> GroupConversationMembers { get; private set; }
    public DbSet<Message> GroupMessages { get; private set; }
    public DbSet<ReplyMessage> ReplyMessages { get; private set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
