using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phymnary.SugarPot.AspNetCore.Entities;

public interface IEntity
{
    [NotMapped]
    EntityDomainStatus DomainStatus { get; }
}

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

    [Key]
    public TKey Id { get; protected init; } = default!;

    public EntityDomainStatus DomainStatus { get; } = new();
}
