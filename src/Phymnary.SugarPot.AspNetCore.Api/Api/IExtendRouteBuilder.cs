using Microsoft.AspNetCore.Builder;

namespace Phymnary.SugarPot.AspNetCore.Api;

public interface IExtendRouteBuilder
{
    RouteHandlerBuilder Extend(RouteHandlerBuilder routeBuilder);
}
