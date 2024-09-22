namespace DomainCommons;

public abstract record AggregateRootEntity<T> : Entity<T>, IAggregateRoot, ISoftDelete, ICreationAuditable, IDeletionAuditable, IModificationAuditable
    where T : struct
{
    public bool IsDeleted { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.Now;
    public DateTimeOffset? DeletedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }

    public virtual void SoftDelete()
    {
        IsDeleted = true;
        DeletedAt = DateTimeOffset.Now;
    }

    public void NotifyModified()
    {
        UpdatedAt = DateTimeOffset.Now;
    }
}
