using System.ComponentModel.DataAnnotations;

namespace Phymnary.SugarPot.AspNetCore.Domain;

public interface IEntity;

public interface IEntity<out TKey> : IEntity
    where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
{
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

    public TKey Id { get; protected init; } = default!;
}
