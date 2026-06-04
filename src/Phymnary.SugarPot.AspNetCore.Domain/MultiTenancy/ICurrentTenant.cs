namespace Phymnary.SugarPot.AspNetCore.MultiTenancy;

public interface ICurrentTenant
{
    Guid? Id { get; }
}
