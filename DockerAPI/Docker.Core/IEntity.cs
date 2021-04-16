namespace Docker.Core
{
    public interface IEntity<T>
    {
        T Id { get; set; }
    }
}
