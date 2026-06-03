using System.Linq.Expressions;

namespace Phymnary.SugarPot.AspNetCore.EntityFrameworkCore.Tests;

public class UnitTest1
{
    class Test 
    { 
        public required int Id { get; init; }
    }

    [Fact]
    public void Test1()
    {
        Expression<Func<Test, int>> expr = x => x.Id;

        var body = expr.Body;
    }
}