namespace Phymnary.SugarPot.AspNetCore.Domain.Entities;

public interface ISoftDelete : IEntity
{
    Guid? DeletedById { get; set; }

    DateTimeOffset? DeletedAt { get; set; }
}
