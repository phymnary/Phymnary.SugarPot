using Phymnary.SugarPot.RoslynComponents.Helpers;

namespace Phymnary.SugarPot.RoslynComponents.Models;

public sealed record Result<TValue>(TValue Value, EquatableArray<SourceDiagnosticInfo> Errors)
    where TValue : IEquatable<TValue>?
{
    public TValue Value { get; } = Value;

    public EquatableArray<SourceDiagnosticInfo> Errors { get; } = Errors;
}
