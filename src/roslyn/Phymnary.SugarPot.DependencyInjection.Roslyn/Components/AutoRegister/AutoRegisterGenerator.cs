using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Phymnary.SugarPot.DependencyInjection.Roslyn.Constants;
using Phymnary.SugarPot.RoslynComponents.Extensions;
using Phymnary.SugarPot.RoslynComponents.Models;

namespace Phymnary.SugarPot.DependencyInjection.Roslyn.Components.AutoRegister;

[Generator(LanguageNames.CSharp)]
public partial class AutoRegisterGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<RegisterServiceInfo> dependencyInfos = context
            .SyntaxProvider.ForAttributeWithMetadataName(
                $"{GeneratorConstant.LibNamespace}.ServiceAttribute",
                static (node, _) => node is ClassDeclarationSyntax,
                static (ctx, token) =>
                {
                    var typeSymbol = (INamedTypeSymbol)ctx.TargetSymbol;

                    return Execute.TryGetDependencyInfo(
                        typeSymbol,
                        ctx.Attributes,
                        token,
                        out var info
                    )
                        ? info
                        : null;
                }
            )
            .Where(info => info is not null)!;

        IncrementalValuesProvider<HierarchyInfo> moduleHierarchies = context
            .SyntaxProvider.ForAttributeWithMetadataName(
                $"{GeneratorConstant.LibNamespace}.AutoAttribute",
                static (node, _) => node is ClassDeclarationSyntax,
                static (ctx, token) =>
                    Execute.TryGetModuleHierarchy(
                        (ClassDeclarationSyntax)ctx.TargetNode,
                        (INamedTypeSymbol)ctx.TargetSymbol,
                        token,
                        out var info
                    )
                        ? info
                        : null
            )
            .Where(module => module is not null)!;

        IncrementalValuesProvider<(
            HierarchyInfo Hierarchy,
            ImmutableArray<RegisterServiceInfo> Infos
        )> grouped = moduleHierarchies.Combine(dependencyInfos.Collect());

        context.RegisterSourceOutput(
            grouped,
            static (src, item) =>
            {
                var registerExpressions = item
                    .Infos.Select(Execute.GetRegistrationExpression)
                    .ToImmutableArray();

                var compilationUnit = BuildSyntax.GetCompilationUnitForDependency(
                    item.Hierarchy,
                    registerExpressions
                );

                src.AddSource($"{item.Hierarchy.FilenameHint}.g.cs", compilationUnit);
            }
        );
    }
}
