using MediatR;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainCommons;

public abstract record Entity<T> : IEntity<T>, IDomainEvents
    where T : struct
{
    [NotMapped]
    private readonly List<INotification> domainEvents = [];

    public abstract T Id { get; protected set; }

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
}
