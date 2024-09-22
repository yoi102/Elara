namespace DomainCommons;

public interface IEntity<T> : IEntity where T : struct
{
    T Id { get; }
}

public interface IEntity
{

}