
using Phymnary.SugarPot.AspNetCore.Entities;
using Phymnary.SugarPot.AspNetCore.MultiTenancy;

namespace Phymnary.SugarPot.AspNetCore.EntityFrameworkCore.Tests;

public class ModelBuilderHelperTests
{
    public class Tenant : Entity<Guid>;

    public class TestEntity : Entity<int>, IMultiTenant
    {
        public Guid TenantId { get; set; }

        public Tenant Tenant { get; set; } = null!;
    }


    [Fact]
    public async Task build_multi_tenancy_entity() {
        
    } 
}
