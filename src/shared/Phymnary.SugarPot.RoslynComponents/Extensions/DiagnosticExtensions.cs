using Microsoft.CodeAnalysis;
using Phymnary.SugarPot.RoslynComponents.Helpers;
using Phymnary.SugarPot.RoslynComponents.Models;

namespace Phymnary.SugarPot.RoslynComponents.Extensions;

public static class DiagnosticExtensions
{
    public static void Add(
        this in ImmutableArrayBuilder<DiagnosticInfo> diagnostics,
        DiagnosticDescriptor descriptor,
        ISymbol symbol,
        params object[] args
    )
    {
        diagnostics.Add(DiagnosticInfo.Create(descriptor, symbol, args));
    }
}
