namespace DomainCommons
{
    public interface IEntity<T> : IEntity where T : struct
    {
        public T Id { get; }
    }

    public interface IEntity
    {

    }
}