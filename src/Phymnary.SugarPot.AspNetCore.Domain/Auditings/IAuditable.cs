namespace Phymnary.SugarPot.AspNetCore.Auditings;

public interface IAuditable
{
    string GetAuditKey();

    DateTimeOffset CreatedAt { get; set; }

    Guid? CreatedById { get; set; }

    DateTimeOffset? UpdatedAt { get; set; }

    Guid? UpdatedById { get; set; }
}
