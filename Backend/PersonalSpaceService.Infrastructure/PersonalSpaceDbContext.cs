﻿using Infrastructure.EFCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalSpaceService.Domain.Entities;

namespace PersonalSpaceService.Infrastructure;

public class PersonalSpaceDbContext : BaseDbContext
{
    public PersonalSpaceDbContext(DbContextOptions options, IMediator mediator) : base(options, mediator)
    {
    }

    public DbSet<Contact> Contacts { get; private set; } = null!;
    public DbSet<Profile> Profiles { get; private set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        modelBuilder.EnableSoftDeletionGlobalFilter();
    }
}