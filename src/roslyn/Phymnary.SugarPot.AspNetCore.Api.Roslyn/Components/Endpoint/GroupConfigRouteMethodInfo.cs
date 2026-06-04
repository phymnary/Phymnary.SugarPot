using System.Collections.Immutable;

namespace Phymnary.SugarPot.AspNetCore.Api.Roslyn.Components.Endpoint;

public record GroupConfigRouteMethodInfo(
    string ContainingType,
    string MethodName,
    ImmutableArray<string> Namespaces
);
