using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Mihaylov.Common.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class CommonNamespaceAnalyzer : DiagnosticAnalyzer
    {
        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            "MIH0001", "Wrong namespace", "Namespace '{0}' must be '{1}'",
            "Naming", DiagnosticSeverity.Error, isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
            => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);

            context.RegisterSyntaxNodeAction(Analyze, SyntaxKind.NamespaceDeclaration, SyntaxKind.FileScopedNamespaceDeclaration);
        }

        private static void Analyze(SyntaxNodeAnalysisContext ctx)
        {
            var node = ctx.Node as BaseNamespaceDeclarationSyntax;
            if (node == null)
            {
                return;
            }

            const string expected = "Mihaylov.Common";

            var actual = node.Name.ToString();
            if (actual != expected)
            {
                ctx.ReportDiagnostic(Diagnostic.Create(Rule, node.Name.GetLocation(), actual, expected));
            }
        }
    }
}
