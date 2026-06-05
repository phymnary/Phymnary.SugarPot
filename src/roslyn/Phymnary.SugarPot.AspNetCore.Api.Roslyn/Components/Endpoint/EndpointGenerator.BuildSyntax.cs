using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Phymnary.SugarPot.RoslynComponents.Helpers;
using Phymnary.SugarPot.RoslynComponents.Models;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Phymnary.SugarPot.AspNetCore.Api.Roslyn.Constants.GeneratorConstant;

namespace Phymnary.SugarPot.AspNetCore.Api.Roslyn.Components.Endpoint;

partial class EndpointGenerator
{
    private static class BuildSyntax
    {
        public static CompilationUnitSyntax GetCompilationUnitForEndpoint(
            HierarchyInfo hierarchyInfo,
            LocalDeclarationStatementSyntax mapRouteSyntax,
            ExpressionStatementSyntax? buildRouteSyntax
        )
        {
            using ImmutableArrayBuilder<MemberDeclarationSyntax> methodSyntaxesBuilder =
                ImmutableArrayBuilder<MemberDeclarationSyntax>.Rent();

            var block = Block().AddStatements(mapRouteSyntax);

            if (buildRouteSyntax is not null)
                block = block.AddStatements(buildRouteSyntax);

            block = block.AddStatements(ReturnStatement(IdentifierName("builder")));

            methodSyntaxesBuilder.Add(
                MethodDeclaration(
                        IdentifierName("global::Microsoft.AspNetCore.Builder.RouteHandlerBuilder"),
                        Identifier("ConfigureRouteBuilder")
                    )
                    .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
                    .WithParameterList(
                        ParameterList(
                            SingletonSeparatedList(
                                Parameter(Identifier("app"))
                                    .WithType(
                                        IdentifierName(
                                            "global::Microsoft.AspNetCore.Routing.IEndpointRouteBuilder"
                                        )
                                    )
                            )
                        )
                    )
                    .WithBody(block)
            );

            var typeDeclarationSyntax = hierarchyInfo.GetCompilationUnit(
                methodSyntaxesBuilder.ToImmutable(),
                BaseList(
                    SingletonSeparatedList<BaseTypeSyntax>(
                        SimpleBaseType(IdentifierName($"global::{LibNamespace}.IEndpoint"))
                    )
                )
            );

            return typeDeclarationSyntax;
        }

        private static readonly SyntaxToken[] PublicStatic =
        {
            Token(SyntaxKind.PublicKeyword),
            Token(SyntaxKind.StaticKeyword),
        };

        private static readonly SyntaxToken[] Public = { Token(SyntaxKind.PublicKeyword) };

        public static CompilationUnitSyntax GetCompilationUnitForSchema(
            HierarchyInfo hierarchyInfo,
            bool isStatic,
            ImmutableArray<ExpressionStatementSyntax> mapNonConstructExpression,
            ImmutableArray<ExpressionStatementSyntax> mapHasConstructExpression
        )
        {
            using ImmutableArrayBuilder<MemberDeclarationSyntax> methodSyntaxesBuilder =
                ImmutableArrayBuilder<MemberDeclarationSyntax>.Rent();

            var method = MethodDeclaration(
                    PredefinedType(Token(SyntaxKind.VoidKeyword)),
                    Identifier("MapEndpoints")
                )
                .AddModifiers(isStatic ? PublicStatic : Public)
                .WithParameterList(
                    ParameterList(
                        SingletonSeparatedList(
                            Parameter(Identifier("app"))
                                .WithType(
                                    IdentifierName(
                                        "global::Microsoft.AspNetCore.Routing.IEndpointRouteBuilder"
                                    )
                                )
                        )
                    )
                )
                .AddBodyStatements([.. mapNonConstructExpression]);

            if (mapHasConstructExpression.Length > 0)
            {
                method = method
                    .AddBodyStatements(
                        LocalDeclarationStatement(
                                VariableDeclaration(
                                        AliasQualifiedName(
                                            IdentifierName(Token(SyntaxKind.GlobalKeyword)),
                                            IdentifierName(
                                                "Microsoft.Extensions.DependencyInjection.IServiceScope"
                                            )
                                        )
                                    )
                                    .WithVariables(
                                        SingletonSeparatedList(
                                            VariableDeclarator(Identifier("scope"))
                                                .WithInitializer(
                                                    EqualsValueClause(
                                                        InvocationExpression(
                                                                MemberAccessExpression(
                                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                                    AliasQualifiedName(
                                                                        IdentifierName(
                                                                            Token(
                                                                                SyntaxKind.GlobalKeyword
                                                                            )
                                                                        ),
                                                                        IdentifierName(
                                                                            "Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions"
                                                                        )
                                                                    ),
                                                                    IdentifierName("CreateScope")
                                                                )
                                                            )
                                                            .AddArgumentListArguments(
                                                                Argument(
                                                                    MemberAccessExpression(
                                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                                        IdentifierName("app"),
                                                                        IdentifierName(
                                                                            "ServiceProvider"
                                                                        )
                                                                    )
                                                                )
                                                            )
                                                    )
                                                )
                                        )
                                    )
                            )
                            .WithUsingKeyword(Token(SyntaxKind.UsingKeyword)),
                        LocalDeclarationStatement(
                            VariableDeclaration(
                                    AliasQualifiedName(
                                        IdentifierName(Token(SyntaxKind.GlobalKeyword)),
                                        IdentifierName("System.IServiceProvider")
                                    )
                                )
                                .WithVariables(
                                    SingletonSeparatedList(
                                        VariableDeclarator(Identifier("serviceProvider"))
                                            .WithInitializer(
                                                EqualsValueClause(
                                                    MemberAccessExpression(
                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                        IdentifierName("scope"),
                                                        IdentifierName("ServiceProvider")
                                                    )
                                                )
                                            )
                                    )
                                )
                        )
                    )
                    .AddBodyStatements([.. mapHasConstructExpression]);
            }

            methodSyntaxesBuilder.Add(method);

            return hierarchyInfo.GetCompilationUnit(methodSyntaxesBuilder.ToImmutable());
        }
    }
}
