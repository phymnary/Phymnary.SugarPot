using System.ComponentModel.DataAnnotations;

namespace AspNetCore.Boilerplate.Domain;

public interface IEntity
{
    object GetKey();
}

public interface IEntity<out TKey> : IEntity
    where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
{
    [Key]
    TKey Id { get; }
}

public abstract class Entity<TKey> : IEntity<TKey>
    where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
{
    protected Entity() { }

    protected Entity(TKey id)
    {
        Id = id;
    }

    [Key]
    public TKey Id { get; protected init; } = default!;

    public object GetKey()
    {
        return Id;
    }
}
