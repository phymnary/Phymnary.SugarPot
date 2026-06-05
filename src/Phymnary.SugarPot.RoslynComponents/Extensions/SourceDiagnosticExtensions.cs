using Microsoft.CodeAnalysis;
using Phymnary.SugarPot.RoslynComponents.Helpers;
using Phymnary.SugarPot.RoslynComponents.Models;

namespace Phymnary.SugarPot.RoslynComponents.Extensions;

public static class SourceDiagnosticExtensions
{
    public static void Add(
        this in ImmutableArrayBuilder<SourceDiagnosticInfo> diagnostics,
        DiagnosticDescriptor descriptor,
        ISymbol symbol,
        params object[] args
    )
    {
        diagnostics.Add(SourceDiagnosticInfo.Create(descriptor, symbol, args));
    }
}
