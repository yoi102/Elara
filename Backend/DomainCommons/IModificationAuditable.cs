namespace DomainCommons;

public interface IModificationAuditable
{
    DateTimeOffset? UpdatedAt { get; }
}
