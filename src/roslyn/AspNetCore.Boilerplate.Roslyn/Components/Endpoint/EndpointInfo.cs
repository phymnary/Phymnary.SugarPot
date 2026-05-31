using System.Collections.Immutable;
using Phymnary.SugarPot.RoslynComponents.Models;

namespace AspNetCore.Boilerplate.Roslyn.Components.Endpoint;

internal record EndpointInfo(
    string TypeName,
    string HttpMethod,
    string HandleMethodName,
    string RoutePatternPropertyName,
    string BuildRouteMethodName,
    bool HasConstructor,
    ImmutableArray<string> Namespaces,
    HierarchyInfo Hierarchy
);
