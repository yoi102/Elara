using MediatR;

namespace DomainCommons;

public interface IDomainEvents
{
    void AddDomainEvent(INotification eventItem);

    void AddDomainEventIfAbsent(INotification eventItem);

    void ClearDomainEvents();

    IEnumerable<INotification> GetDomainEvents();
}