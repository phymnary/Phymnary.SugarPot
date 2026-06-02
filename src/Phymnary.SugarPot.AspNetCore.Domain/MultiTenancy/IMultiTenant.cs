namespace Phymnary.SugarPot.AspNetCore.MultiTenancy;

public interface IMultiTenant
{
    Guid TenantId { get; set; }
}
