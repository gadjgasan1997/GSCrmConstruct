using System.Linq;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxKind;
using System;

namespace GSCrmLibrary.CodeGeneration
{
    public static class CodGenTreesUtils
    {
        public static bool TryGetDirective(string directiveName, ref DirectiveTriviaSyntax directive)
        {
            while(directive != null && directive.ToString() != directiveName && directive.GetNextDirective() != null)
                directive = directive.GetNextDirective();
            if (directive?.ToString() != directiveName)
                return false;
            else return true;
        }

        public static string GetDirectiveName(DirectiveTriviaSyntax directive)
            => directive.ToString().Split("#" + directive.DirectiveNameToken)[1].TrimStart();

        private static SyntaxKind GetEndOfDirectiveKind(SyntaxKind directiveKind)
            => new Dictionary<SyntaxKind, SyntaxKind>()
            {
                { RegionKeyword, EndRegionKeyword },
                { IfKeyword, EndIfKeyword },
            }[directiveKind];

        public static bool IsEmptyDirective(SyntaxNode rootNode, DirectiveTriviaSyntax directive)
        {
            MemberDeclarationSyntax member = rootNode
                .DescendantNodes()
                .OfType<MemberDeclarationSyntax>()
                .FirstOrDefault(t => t.GetLeadingTrivia().ToFullString().Contains(directive.ToString()));
            if (member == null)
                return true;
            else return false;
        }

        public static IEnumerable<SyntaxNode> GetSiblingNodes(SyntaxNode syntaxNode) => syntaxNode.Parent.ChildNodes();

        public static IEnumerable<SyntaxNode> GetNodesBetween(SyntaxNode firstNode, SyntaxNode lastNode)
        {
            if (firstNode != null)
            {
                IEnumerable<SyntaxNode> siblingNodes = GetSiblingNodes(firstNode);
                if (siblingNodes != null)
                    return siblingNodes
                        .SkipWhile(n => n != firstNode)
                        .TakeWhile(n => n != lastNode)
                        .Concat(new List<SyntaxNode>() { siblingNodes.FirstOrDefault(n => n == lastNode) });
                else return new List<SyntaxNode>();
            }
            else return new List<SyntaxNode>();
        }

        public static PropertyDeclarationSyntax GenerateAutoProp(string propertyType, string propertyName, bool isNullable, params SyntaxKind[] modifiersList)
        {
            if (string.IsNullOrWhiteSpace(propertyType))
                propertyType = "dynamic";
            if (isNullable)
                propertyType += "?";
            return PropertyDeclaration(
                    ParseTypeName(propertyType).WithTrailingTrivia(Whitespace(" ")),
                    ParseToken(propertyName).WithTrailingTrivia(Whitespace(" ")))
                .WithModifiers(
                    TokenList(GenerateModifiers(modifiersList)))
                        .WithLeadingTrivia(GenerateTabs(2))
                .WithAccessorList(
                    AccessorList(
                        Token(OpenBraceToken).WithTrailingTrivia(Whitespace(" ")), List(
                            new List<AccessorDeclarationSyntax>()
                            {
                                AccessorDeclaration(GetAccessorDeclaration)
                                    .WithSemicolonToken(Token(SemicolonToken))
                                    .WithTrailingTrivia(Whitespace(" ")),
                                AccessorDeclaration(SetAccessorDeclaration)
                                    .WithSemicolonToken(Token(SemicolonToken))
                                    .WithTrailingTrivia(Whitespace(" ")),
                            }),
                        Token(CloseBraceToken).WithTrailingTrivia(LineFeed)));
        }
        
        public static PropertyDeclarationSyntax GeneratePropWithRegion(PropertyDeclarationSyntax property, SyntaxKind directiveKind, string directiveName, int depth)
        {
            return property
                .WithLeadingTrivia(
                    TriviaList(
                        new List<SyntaxTrivia>()
                        {
                            Trivia(
                                RegionDirectiveTrivia(
                                    Token(HashToken).WithLeadingTrivia(GenerateTabs(depth)),
                                    Token(directiveKind).WithTrailingTrivia(Whitespace(" ")),
                                    Token(EndOfDirectiveToken)
                                        .WithLeadingTrivia(PreprocessingMessage(directiveName))
                                        .WithTrailingTrivia(LineFeed),
                                    true))
                        }.Concat(property.GetLeadingTrivia())))
                .WithTrailingTrivia(
                    TriviaList(
                        new List<SyntaxTrivia>()
                        {
                            Trivia(
                                EndRegionDirectiveTrivia(
                                    Token(HashToken).WithLeadingTrivia(
                                        TriviaList(
                                            Whitespace("\n"),
                                            GenerateTabs(depth))),
                                    Token(GetEndOfDirectiveKind(directiveKind)),
                                    Token(EndOfDirectiveToken),
                                    true))
                        }.Concat(property.GetTrailingTrivia())));
        }

        public static IEnumerable<SyntaxToken> GenerateModifiers(params SyntaxKind[] modifiersList)
        {
            foreach (SyntaxKind modifier in modifiersList)
                yield return Token(modifier).WithTrailingTrivia(Whitespace(" "));
        }

        public static SyntaxTrivia GenerateTabs(int depth)
        {
            string whiteSpaceString = string.Empty;
            for (int i = 0; i < depth; i++)
                whiteSpaceString += "    ";
            return Whitespace(whiteSpaceString);
        }

        public static SyntaxNode InsertPropInTopOfClass(SyntaxNode rootNode, string className, PropertyDeclarationSyntax property)
        {
            ClassDeclarationSyntax classNode = rootNode
                .DescendantNodes()
                .OfType<ClassDeclarationSyntax>()
                .FirstOrDefault(n => n.Identifier.ValueText == className);
            PropertyDeclarationSyntax firstProp = classNode
                .DescendantNodes()
                .OfType<PropertyDeclarationSyntax>()
                .FirstOrDefault();

            // Если в классе есть свойство, новое добавляется перед ним
            if (firstProp != null)
            {
                rootNode = rootNode.InsertNodesBefore(
                    firstProp,
                    new List<PropertyDeclarationSyntax>()
                    {
                        property.WithTrailingTrivia(property.GetTrailingTrivia())
                    });
            }

            else
            {
                rootNode = rootNode.ReplaceNode(
                    classNode,
                    classNode.WithMembers(List(
                        new List<MemberDeclarationSyntax>()
                        {
                            property
                        })));
            }

            return rootNode;
        }

        public static SyntaxNode RemoveNodeKeepTrivia(SyntaxNode rootNode, string entityName)
        {
            PropertyDeclarationSyntax property = rootNode
                .DescendantNodes()
                .OfType<PropertyDeclarationSyntax>()
                .FirstOrDefault(n => n.Identifier.ValueText == entityName);

            if (property != null)
            {
                rootNode = rootNode.ReplaceNode(
                    property,
                    property
                        .WithoutLeadingTrivia()
                        .WithLeadingTrivia(property.GetLeadingTrivia().SkipLast(1)));

                rootNode = rootNode.RemoveNode(
                    rootNode
                        .DescendantNodes()
                        .OfType<PropertyDeclarationSyntax>()
                        .FirstOrDefault(n => n.Identifier.ValueText == entityName),
                    SyntaxRemoveOptions.KeepLeadingTrivia);
            }

            return rootNode;
        }

        public static IEnumerable<SyntaxToken> GetSeparators(int count, SyntaxKind separatorkind)
        {
            for (int i = 0; i < count; i++)
                yield return Token(separatorkind);
        }
        public static IEnumerable<SyntaxToken> GetSeparators(int count, SyntaxKind separatorkind, SyntaxTrivia trivia)
        {
            for (int i = 0; i < count; i++)
                yield return Token(separatorkind).WithTrailingTrivia(TriviaList(trivia));
        }
    }
}
