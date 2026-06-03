using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phymnary.SugarPot.AspNetCore.Entities;

public interface IEntity
{
    [NotMapped]
    EntityDomainStatus DomainStatus { get; }
}

public interface IHasKey<TKey>
    where TKey : notnull
{
    TKey GetKey();
}

public interface IEntity<TKey> : IEntity, IHasKey<TKey>
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

    [Key]
    public TKey Id { get; protected init; } = default!;

    public EntityDomainStatus DomainStatus { get; } = new();

    public TKey GetKey() => Id;
}
