namespace Phymnary.SugarPot.AspNetCore.Auditings;

public class AuditingEntityMapper<TConcrete, TImplement>
    where TImplement : class
{
    public required Func<TConcrete, TImplement> Map { get; init; }
}
