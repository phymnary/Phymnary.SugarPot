namespace Phymnary.SugarPot.AspNetCore.MultiTenancy;

internal class HttpContextCurrentTenant : ICurrentTenant
{
    public Guid? Id { get; set; }
}
