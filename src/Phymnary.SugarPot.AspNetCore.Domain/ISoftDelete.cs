namespace Phymnary.SugarPot.AspNetCore.Domain;

public interface ISoftDelete
{
    Guid? DeletedById { get; set; }

    DateTimeOffset? DeletedAt { get; set; }

    public void SoftDeleted()
    {
        DeletedAt = DateTimeOffset.MaxValue;
    }
}
