namespace DomainCommons;

public interface IDeletionAuditable
{
    DateTimeOffset? DeletedAt { get; }
}
