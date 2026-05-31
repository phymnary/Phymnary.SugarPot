namespace Phymnary.SugarPot.DependencyInjection.Roslyn.Components.AutoRegister;

internal sealed record RegisterServiceInfo(
    string ServiceTypeName,
    string ImplementationTypeName,
    string Lifetime
);
