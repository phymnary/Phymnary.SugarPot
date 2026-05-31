namespace Phymnary.SugarPot.AspNetCore.EntityFrameworkCore;

public static class EfFeatureFlags
{
    public static bool IsMultiTenantEnable { get; set; }

    public static bool IsAuditingEnable { get; set; }
}
