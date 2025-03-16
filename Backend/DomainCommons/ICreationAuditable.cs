namespace DomainCommons;

public interface ICreationAuditable
{
    DateTimeOffset CreatedAt { get; }
}
