using Phymnary.SugarPot.AspNetCore.Entities;

namespace Phymnary.SugarPot.AspNetCore.EntityFrameworkCore.Tests;

public class UnitTest1
{
    class Test : Entity<int>;

    [Fact]
    public void Test1()
    {
        var lookup = new EntitySchemaLookup();
        lookup.Add<Test>();
    }
}
