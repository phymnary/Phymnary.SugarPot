namespace Phymnary.SugarPot.AspNetCore.Domain.Auditings;

public class AuditingEntityMapper<TConcrete, TImplement>
    where TImplement : class
{
    public required Func<TConcrete, TImplement> Map { get; init; }
}
