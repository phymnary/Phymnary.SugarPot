using Microsoft.CodeAnalysis;
using Phymnary.SugarPot.DependencyInjection.Roslyn.Components.AutoRegister;

namespace Phymnary.SugarPot.DependencyInjection.Roslyn.Diagnostics;

internal static class DiagnosticDescriptors
{
    public static readonly DiagnosticDescriptor InvalidClassDecorationForDependency = new(
        "SPDI0001",
        "Invalid Class Decoration for [Dependency] to be registered",
        "The Dependency {0} could not be registered as a service because it's a abstract or generic class",
        nameof(AutoRegisterGenerator),
        DiagnosticSeverity.Error,
        true,
        "Class annotated with [Dependency] must not be abstract or generic.",
        ""
    );
}
