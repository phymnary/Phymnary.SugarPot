using Microsoft.CodeAnalysis;
using Phymnary.SugarPot.AspNetCore.Api.Roslyn.Components.Endpoint;

namespace Phymnary.SugarPot.AspNetCore.Api.Roslyn.Diagnostics;

internal static class DiagnosticDescriptors
{
    public static readonly DiagnosticDescriptor MissingHandleMethodForEndpoint = new(
        "SPAPI001",
        "Missing \"HandleAsync\" method for [Endpoint]",
        "The Endpoint {0} is missing \"HandleAsync\" method",
        nameof(EndpointGenerator),
        DiagnosticSeverity.Error,
        true,
        "Class annotated with [Endpoint] must have a method named \"HandleAsync\".",
        ""
    );
}
