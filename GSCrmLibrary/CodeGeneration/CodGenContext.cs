using System.Linq;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxKind;
using static GSCrmLibrary.CodeGeneration.CodGenUtils;
using static GSCrmLibrary.CodeGeneration.CodGenTreesUtils;
using static GSCrmLibrary.Configuration.EntitiesConfig;
using static GSCrmLibrary.Configuration.ApplicationConfig;

namespace GSCrmLibrary.CodeGeneration
{
    public static class CodGenContext
    {
        public static void AddEntityToContext(string entityName)
        {
            SyntaxTree syntaxTree = GetDocumentTree(GSCrmApplication, GetEntityExPath(GSCrmApplication, "GSAppContext", "Context"));
            SyntaxNode rootNode = syntaxTree.GetRoot();
            if (rootNode
                    .DescendantNodes()
                    .OfType<PropertyDeclarationSyntax>()
                    .SingleOrDefault(n => n.Identifier.ValueText == entityName) == null)
            {
                ClassDeclarationSyntax classNode = rootNode
                    .DescendantNodes()
                    .OfType<ClassDeclarationSyntax>()
                    .SingleOrDefault(n => n.Identifier.ValueText == "GSAppContext");
                rootNode = rootNode.ReplaceNode(classNode, classNode.WithMembers(List(
                    new List<SyntaxNode>()
                    {
                        GenerateAutoProp(
                            "DbSet<" + entityName + ">",
                            entityName,
                            false, PublicKeyword)
                    }.Concat(classNode.Members))));
                syntaxTree = SyntaxTree(rootNode).WithFilePath(syntaxTree.FilePath);

                // Запись в файл
                WriteTree(GSCrmApplication, "GSAppContext", "Context", syntaxTree);
                Compile("GSCrmContext", syntaxTree);
            }
        }

        public static void DeleteEntityFromContext(string entityName)
        {
            // Получение компиляции и дерева
            SyntaxTree tree = GetFreshEntitySyntaxTree(GSCrmApplication, "GSAppContext", "Context");
            if (tree != null)
            {
                string filePath = tree.FilePath;
                tree = SyntaxTree(RemoveNodeKeepTrivia(tree.GetRoot(), entityName)).WithFilePath(filePath);

                // Запись дерева и компиляция
                WriteTree(GSCrmApplication, "GSAppContext", "Context", tree);
                Compile("GSCrmContext", tree);
            }
        }
    }
}