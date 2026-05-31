using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Phymnary.SugarPot.AspNetCore.Api;

public interface IEndpoint
{
    RouteHandlerBuilder ConfigureRouteBuilder(IEndpointRouteBuilder app);
}
