using Microsoft.AspNetCore.Builder;
using Phymnary.SugarPot.AspNetCore.Api;

namespace Phymnary.SugarPot.AspNetCore.IntegrationTests;

public static class ApiGroup
{
    [RoutePattern]
    public static string GetRouteName<TEndpoint>(TEndpoint endpoint, string? prefix = null)
        where TEndpoint : class, IEndpoint
    {
        return "";
    }

    [RouteBuilder]
    public static void BuildRoute(RouteHandlerBuilder builder)
    {
        builder.RequireAuthorization();
    }
}
