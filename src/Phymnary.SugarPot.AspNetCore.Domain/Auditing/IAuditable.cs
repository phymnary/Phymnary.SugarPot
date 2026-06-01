namespace Phymnary.SugarPot.AspNetCore.Domain.Auditing;

public interface IAuditable
{
    DateTimeOffset CreatedAt { get; set; }

    Guid? CreatedById { get; set; }

    DateTimeOffset? UpdatedAt { get; set; }

    Guid? UpdatedById { get; set; }
}
