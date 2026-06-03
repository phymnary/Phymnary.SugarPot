namespace Phymnary.SugarPot.AspNetCore.Entities;

public interface ISoftDelete : IEntity
{
    Guid? DeletedById { get; set; }

    DateTimeOffset? DeletedAt { get; set; }

    public void Delete()
    {
        DomainStatus.SoftDelete();
    }
}
