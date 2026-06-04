using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Phymnary.SugarPot.AspNetCore.Api.Roslyn.Constants;
using Phymnary.SugarPot.AspNetCore.Api.Roslyn.Diagnostics;
using Phymnary.SugarPot.RoslynComponents.Extensions;
using Phymnary.SugarPot.RoslynComponents.Models;

namespace Phymnary.SugarPot.AspNetCore.Api.Roslyn.Components.Endpoint;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class EndpointAnalyzer : DiagnosticAnalyzer
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
                $"{GeneratorConstant.LibNamespace}.EndpointAttribute"
            )
        )
            return;

        if (
            !typeSymbol
                .GetMembers()
                .Any(member => member is IMethodSymbol && member.Name == "HandleAsync")
        )
            context.ReportDiagnostic(
                DiagnosticInfo
                    .Create(
                        DiagnosticDescriptors.MissingHandleMethodForEndpoint,
                        typeSymbol,
                        string.Empty
                    )
                    .ToDiagnostic()
            );
    }

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
        ImmutableArray.Create(DiagnosticDescriptors.MissingHandleMethodForEndpoint);
}
