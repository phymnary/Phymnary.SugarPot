namespace Phymnary.SugarPot.AspNetCore.Domain.MultiTenancy;

public interface IMultiTenant
{
    Guid TenantId { get; set; }
}
