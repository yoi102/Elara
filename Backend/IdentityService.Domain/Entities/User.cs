using DomainCommons;
using DomainCommons.EntityStronglyIds;
using IdentityService.Domain.Events;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityService.Domain.Entities;

public class User : IdentityUser<UserId>, ISoftDelete, ICreationAuditable, IDeletionAuditable, IDomainEvents
{
    [NotMapped]
    private readonly List<INotification> domainEvents = [];

    public User(string name, string email)
    {
        Id = UserId.New();
        Email = email;
        UserName = name;
        CreatedAt = DateTimeOffset.Now;
        AddDomainEvent(new UserCreatedEvent(this));
    }

    private User()
    {
    }
    private string? passwordHash;

    public override string? PasswordHash
    {
        get => passwordHash;
        set
        {
            if (passwordHash != value) // 确保发生变化时触发事件
            {
                passwordHash = value;

                // 触发事件逻辑
                AddDomainEventIfAbsent(new UserPasswordChangedEvent(this));
            }
        }
    }

    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? DeletedAt { get; private set; }
    public bool IsDeleted { get; private set; }

    public void AddDomainEvent(INotification eventItem)
    {
        domainEvents.Add(eventItem);
    }

    public void AddDomainEventIfAbsent(INotification eventItem)
    {
        if (!domainEvents.Contains(eventItem))
        {
            domainEvents.Add(eventItem);
        }
    }

    public void ClearDomainEvents()
    {
        domainEvents.Clear();
    }

    public IEnumerable<INotification> GetDomainEvents()
    {
        return domainEvents;
    }

    public void SoftDelete()
    {
        IsDeleted = true;
        DeletedAt = DateTimeOffset.Now;
        AddDomainEventIfAbsent(new UserDeletedEvent(this));
    }
}
