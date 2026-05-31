using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Phymnary.SugarPot.DependencyInjection.Roslyn.Constants;
using Phymnary.SugarPot.DependencyInjection.Roslyn.Diagnostics;
using Phymnary.SugarPot.RoslynComponents.Extensions;
using Phymnary.SugarPot.RoslynComponents.Helpers;
using Phymnary.SugarPot.RoslynComponents.Models;

namespace Phymnary.SugarPot.DependencyInjection.Roslyn.Components.AutoRegister;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class AutoRegisterAnalyzer : DiagnosticAnalyzer
{
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeDependencySyntax, SyntaxKind.ClassDeclaration);
    }

    private static void AnalyzeDependencySyntax(SyntaxNodeAnalysisContext context)
    {
        if (context.Node is not ClassDeclarationSyntax)
            return;

        if (
            context.ContainingSymbol is not INamedTypeSymbol typeSymbol
            || !typeSymbol.HasAttributeWithFullyQualifiedMetadataName(
                $"{GeneratorConstant.LibNamespace}.ServiceAttribute"
            )
        )
            return;

        var typeName = typeSymbol.GetFullyQualifiedMetadataName();

        using var builder = ImmutableArrayBuilder<DiagnosticInfo>.Rent();

        if (typeSymbol is not { IsAbstract: false, IsGenericType: false })
            builder.Add(
                DiagnosticDescriptors.InvalidClassDecorationForDependency,
                typeSymbol,
                typeName
            );

        foreach (var diagnostic in builder.ToImmutable())
            context.ReportDiagnostic(diagnostic.ToDiagnostic());
    }

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
        ImmutableArray.Create(DiagnosticDescriptors.InvalidClassDecorationForDependency);
}
