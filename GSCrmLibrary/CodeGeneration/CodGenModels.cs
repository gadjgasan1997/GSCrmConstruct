using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxKind;
using static GSCrmLibrary.CodeGeneration.CodGenUtils;
using static GSCrmLibrary.CodeGeneration.CodGenTreesUtils;
using GSCrmLibrary.Models.TableModels;
using Table = GSCrmLibrary.Models.TableModels.Table;
using GSCrmLibrary.Data;
using BusinessComponent = GSCrmLibrary.Models.TableModels.BusinessComponent;
using Applet = GSCrmLibrary.Models.TableModels.Applet;
using static GSCrmLibrary.Configuration.EntitiesConfig;
using static GSCrmLibrary.Configuration.ApplicationConfig;
using static GSCrmLibrary.Utils;

namespace GSCrmLibrary.CodeGeneration
{
    public static class CodGenModels
    {
        public static void GenerateModel(MainContext context, Type entityType, Guid recordId)
        {
            SyntaxTree syntaxTree;
            string entityName = string.Empty;
            IEnumerable<MemberDeclarationSyntax> members = new List<MemberDeclarationSyntax>();
            switch (entityType.Name)
            {
                case "Table":
                    Table table = context.Tables
                        .AsNoTracking()
                        .Include(tc => tc.TableColumns)
                        .FirstOrDefault(i => i.Id == recordId);
                    entityName = table.Name;
                    members = CreateTableColumns(table.TableColumns, context);
                    break;
                case "BusinessComponent":
                    BusinessComponent busComp = context.BusinessComponents
                        .AsNoTracking()
                        .Include(f => f.Fields)
                            .ThenInclude(tc => tc.TableColumn)
                        .Include(f => f.Fields)
                            .ThenInclude(tc => tc.JoinColumn)
                        .FirstOrDefault(i => i.Id == recordId);
                    entityName = busComp.Name;
                    members = CreateFields(busComp.Fields);
                    break;
                case "Applet":
                    Applet applet = context.Applets
                        .AsNoTracking()
                        .Include(c => c.Controls)
                            .ThenInclude(f => f.Field)
                        .Include(c => c.Columns)
                            .ThenInclude(f => f.Field)
                        .FirstOrDefault(i => i.Id == recordId);
                    entityName = applet.Name;
                    members = CreateColumns(applet.Columns, context).Concat(CreateControls(applet.Controls, context));
                    break;
            }
            string permissibleName = GetPermissibleName(entityName);
            if (TryGetModel(GSCrmApplication, permissibleName, entityType.Name, out syntaxTree))
            {
                SyntaxNode rootNode = syntaxTree.GetRoot();
                ClassDeclarationSyntax classNode = rootNode
                    .DescendantNodes()
                    .OfType<ClassDeclarationSyntax>()
                    .SingleOrDefault(n => n.Identifier.ValueText == permissibleName);
                List<PropertyDeclarationSyntax> interProps = new List<PropertyDeclarationSyntax>();
                classNode.Members.OfType<PropertyDeclarationSyntax>().ToList().ForEach(property =>
                {
                    if (!members.Contains(property) && !property.Identifier.ValueText.StartsWith(SystemPrefix))
                        interProps.Add(property);
                });
                rootNode = rootNode.ReplaceNode(classNode, classNode.WithMembers(List(members.Except(interProps))));
                syntaxTree = SyntaxTree(rootNode).WithFilePath(syntaxTree.FilePath);
            }
            else
            {
                syntaxTree = CreateModel(permissibleName, entityType, members);
                if (entityType == typeof(Table))
                    Compile("GSCrmApplicationContext", syntaxTree);
            }
            WriteTree(GSCrmApplication, permissibleName, entityType.Name, syntaxTree);
            Compile("GSCrm" + entityType.Name, syntaxTree);
        }
        
        private static SyntaxTree CreateModel(string entityName, Type entityType, IEnumerable<MemberDeclarationSyntax> members)
        {
            return SyntaxTree(
                CompilationUnit()
                    .AddUsings(
                        UsingDirective(
                            ParseName("System").WithLeadingTrivia(Whitespace(" "))
                        ).WithTrailingTrivia(LineFeed),
                        UsingDirective(
                            ParseName("System.Collections.Generic").WithLeadingTrivia(Whitespace(" "))
                        ).WithTrailingTrivia(LineFeed),
                        UsingDirective(
                            ParseName("System.ComponentModel.DataAnnotations.Schema").WithLeadingTrivia(Whitespace(" "))
                        ).WithTrailingTrivia(LineFeed),
                        UsingDirective(
                            ParseName("System.Text.Json.Serialization").WithLeadingTrivia(Whitespace(" "))
                        ).WithTrailingTrivia(LineFeed),
                        UsingDirective(
                            ParseName("GSCrmApplication.Models.MainEntities").WithLeadingTrivia(Whitespace(" "))
                        ).WithTrailingTrivia(Whitespace("\n\n")))
                    .AddMembers(
                        NamespaceDeclaration(
                            QualifiedName(
                                QualifiedName(
                                    IdentifierName("GSCrmApplication"),
                                    Token(DotToken),
                                    IdentifierName("Models")),
                                Token(DotToken),
                                IdentifierName(entityType.Name == "Table" ? "TableModels" : entityType.Name == "BusinessComponent" ? "BusinessComponentModels" : "AppletModels"))
                                    .WithTrailingTrivia(Whitespace("\n")))
                            .WithNamespaceKeyword(Token(NamespaceKeyword).WithTrailingTrivia(Whitespace(" ")))
                            .WithMembers(List(
                                new List<MemberDeclarationSyntax>()
                                {
                                    ClassDeclaration(ParseToken(entityName).WithTrailingTrivia(Whitespace(" ")))
                                        .WithModifiers(
                                            TokenList(
                                                Token(PublicKeyword)
                                                    .WithLeadingTrivia(Tab)
                                                    .WithTrailingTrivia(Whitespace(" "))))
                                            .WithKeyword(Token(ClassKeyword).WithTrailingTrivia(Whitespace(" ")))
                                        .WithBaseList(
                                            BaseList(
                                                SingletonSeparatedList<BaseTypeSyntax>(
                                                    SimpleBaseType(
                                                        IdentifierName(
                                                            Identifier(
                                                                TriviaList(Whitespace(" ")),
                                                                GetEntityBaseType(entityType.Name),
                                                                TriviaList(LineFeed)))))))
                                        .WithMembers(List(members))
                                        .WithOpenBraceToken(Token(OpenBraceToken)
                                            .WithLeadingTrivia(Tab)
                                            .WithTrailingTrivia(LineFeed))
                                        .WithCloseBraceToken(Token(CloseBraceToken)
                                            .WithLeadingTrivia(Tab)
                                            .WithTrailingTrivia(LineFeed))
                                }))
                            .WithOpenBraceToken(Token(OpenBraceToken).WithTrailingTrivia(LineFeed))
                            .WithCloseBraceToken(Token(CloseBraceToken).WithTrailingTrivia(LineFeed))))
                .WithFilePath(GetEntityExPath(GSCrmApplication, entityName, entityType.Name));
        }

        private static IEnumerable<PropertyDeclarationSyntax> CreateTableColumns(IEnumerable<TableColumn> tableColumns, MainContext context)
        {
            List<PropertyDeclarationSyntax> properties = new List<PropertyDeclarationSyntax>();
            foreach (TableColumn tableColumn in tableColumns)
            {
                if (!IsSystemProperty(tableColumn.Name))
                {
                    PropertyDeclarationSyntax autoProp = GenerateAutoProp(tableColumn.Type, tableColumn.Name, tableColumn.IsNullable, new SyntaxKind[] { PublicKeyword });

                    // Составление внешнего ключа
                    if (tableColumn.IsForeignKey)
                    {
                        Table foreignTable = context.Tables.AsNoTracking().FirstOrDefault(i => i.Id == tableColumn.ForeignTableId);
                        properties.Add(GetNavProperty(foreignTable, tableColumn));
                        UpdateForeignProperty(foreignTable, tableColumn);
                    }
                    properties.Add(autoProp);
                }
            }
            return properties;
        }

        private static IEnumerable<PropertyDeclarationSyntax> CreateFields(IEnumerable<Field> fields)
        {
            foreach (Field field in fields)
            {
                if (!IsSystemProperty(field.Name) && !field.IsCalculate)
                {
                    TableColumn column = field.TableColumn ?? field.JoinColumn;
                    yield return GenerateAutoProp(column.Type, field.Name, column != null ? column.IsNullable : false, new SyntaxKind[] { PublicKeyword });
                }
                else if (field.IsCalculate)
                    yield return GenerateAutoProp("string", field.Name, true, new SyntaxKind[] { PublicKeyword });
            }
        }

        private static IEnumerable<PropertyDeclarationSyntax> CreateColumns(IEnumerable<Column> columns, MainContext context)
        {
            foreach (Column column in columns)
            {
                if (!IsSystemProperty(column.Name) && (column.Field == null || !column.Field.IsCalculate))
                {
                    Guid? tableColumnId = column.Field?.TableColumnId ?? column.Field?.JoinColumnId;
                    TableColumn tableColumn = tableColumnId == null ? null : context.TableColumns.AsNoTracking().FirstOrDefault(i => i.Id == tableColumnId);
                    yield return GenerateAutoProp(tableColumn?.Type, column.Name, tableColumn == null ? false : tableColumn.IsNullable, new SyntaxKind[] { PublicKeyword })
                        .WithAttributeLists(GenerateJsonPropertyName(column.Name));
                }
                else if (column.Field != null && column.Field.IsCalculate)
                    yield return GenerateAutoProp("string", column.Name, true, new SyntaxKind[] { PublicKeyword })
                        .WithAttributeLists(GenerateJsonPropertyName(column.Name));
            }
        }

        private static IEnumerable<PropertyDeclarationSyntax> CreateControls(IEnumerable<Control> controls, MainContext context)
        {
            foreach (Control control in controls)
            {
                if (!IsSystemProperty(control.Name) && (control.Field == null || !control.Field.IsCalculate))
                {
                    Guid? tableColumnId = control.Field?.TableColumnId ?? control.Field?.JoinColumnId;
                    TableColumn tableColumn = tableColumnId == null ? null : context.TableColumns.AsNoTracking().FirstOrDefault(i => i.Id == tableColumnId);
                    yield return GenerateAutoProp(tableColumn?.Type, control.Name, tableColumn != null ? tableColumn.IsNullable : false, new SyntaxKind[] { PublicKeyword })
                        .WithAttributeLists(GenerateJsonPropertyName(control.Name));
                }
                
                else if (control.Field != null && control.Field.IsCalculate)
                {
                    yield return GenerateAutoProp("string", control.Name, true, new SyntaxKind[] { PublicKeyword })
                        .WithAttributeLists(GenerateJsonPropertyName(control.Name));
                }
            }
        }

        private static SyntaxList<AttributeListSyntax> GenerateJsonPropertyName(string propertName)
        {
            return List(
                new List<AttributeListSyntax>()
                {
                    AttributeList(
                        Token(OpenBracketToken).WithLeadingTrivia(GenerateTabs(2)),
                        default,
                        SeparatedList(new List<AttributeSyntax>()
                        {
                            Attribute(
                                IdentifierName("JsonPropertyName"),
                                AttributeArgumentList(
                                    Token(OpenParenToken),
                                    SeparatedList(
                                        new List<AttributeArgumentSyntax>()
                                        {
                                            AttributeArgument(LiteralExpression(StringLiteralExpression, Literal(propertName))),
                                        }),
                                    Token(CloseParenToken))),
                        }),
                        Token(CloseBracketToken)).WithTrailingTrivia(LineFeed)
                });
        }

        private static PropertyDeclarationSyntax GetNavProperty(Table foreignTable, TableColumn column)
        {
            string permissibleName = GetPermissibleName(foreignTable.Name);
            return GenerateAutoProp(permissibleName, SystemPrefix + permissibleName, column.IsNullable, PublicKeyword)
                .WithAttributeLists(List(
                    new List<AttributeListSyntax>()
                    {
                        // Атрибут с типом контекста
                        AttributeList(
                            Token(OpenBracketToken).WithLeadingTrivia(GenerateTabs(2)),
                            default,
                            SeparatedList(
                                new List<AttributeSyntax>()
                                {
                                    Attribute(
                                        IdentifierName("ForeignKey"),
                                        AttributeArgumentList(
                                            Token(OpenParenToken),
                                            SeparatedList(
                                                new List<AttributeArgumentSyntax>()
                                                {
                                                    AttributeArgument(
                                                        LiteralExpression(
                                                            StringLiteralExpression,
                                                            Literal(column.Name))),
                                                }),
                                            Token(CloseParenToken))),
                                }),
                            Token(CloseBracketToken))
                        .WithTrailingTrivia(LineFeed)
                    }));
        }

        public static void UpdateForeignProperty(Table foreignTable, TableColumn tableColumn)
        {
            SyntaxTree foreignTree;
            string permissibleName = GetPermissibleName(foreignTable.Name);
            if (TryGetModel(GSCrmApplication, permissibleName, "Table", out foreignTree))
            {
                SyntaxNode rootNode = foreignTree.GetRoot();
                ClassDeclarationSyntax classNode = rootNode
                    .DescendantNodes()
                    .OfType<ClassDeclarationSyntax>()
                    .SingleOrDefault(n => n.Identifier.ValueText == permissibleName);

                string newNavPropName = SystemPrefix + (tableColumn.ExtencionType == "OneToOne" ? tableColumn.Table.Name : tableColumn.Table.Name + "s");

                // Старое навигационное свойство во внешнем классе
                PropertyDeclarationSyntax oldNavProp = classNode
                    .ChildNodes()
                    .OfType<PropertyDeclarationSyntax>()
                    .FirstOrDefault(t => t.Type.ToString().Contains(tableColumn.Table.Name));

                // Удаление старого навигационного свойства во внешнем классе
                if (oldNavProp != null)
                    classNode = classNode.RemoveNode(oldNavProp, SyntaxRemoveOptions.KeepNoTrivia);

                // Замена старого узла на новый
                rootNode = rootNode.ReplaceNode(rootNode
                    .DescendantNodes()
                    .OfType<ClassDeclarationSyntax>()
                    .SingleOrDefault(n => n.Identifier.ValueText == permissibleName),
                    classNode.WithMembers(List(
                        classNode.Members.Append(
                            GenerateAutoProp(
                                tableColumn.ExtencionType == "OneToOne" ? tableColumn.Table.Name : "List<" + tableColumn.Table.Name + ">",
                                newNavPropName, false, PublicKeyword)))));
                foreignTree = SyntaxTree(rootNode).WithFilePath(foreignTree.FilePath);

                // Запись и компиляция
                WriteTree(GSCrmApplication, permissibleName, "Table", foreignTree);
                Compile("GSCrmTable", foreignTree);
            }
        }

        public static void DeleteForeignProperty(Table foreignTable, Table currentTable)
        {
            SyntaxTree foreignTree;
            string permissibleName = GetPermissibleName(foreignTable.Name);
            if (TryGetModel(GSCrmApplication, permissibleName, "Table", out foreignTree))
            {
                SyntaxNode rootNode = foreignTree.GetRoot();
                ClassDeclarationSyntax classNode = rootNode
                    .DescendantNodes()
                    .OfType<ClassDeclarationSyntax>()
                    .SingleOrDefault(n => n.Identifier.ValueText == permissibleName);

                // Старое навигационное свойство во внешнем классе
                PropertyDeclarationSyntax oldNavProp = classNode
                    .ChildNodes()
                    .OfType<PropertyDeclarationSyntax>()
                    .FirstOrDefault(t => t.Type.ToString().Contains(currentTable.Name));

                // Удаление старого навигационного свойства во внешнем классе
                if (oldNavProp != null)
                    classNode = classNode.RemoveNode(oldNavProp, SyntaxRemoveOptions.KeepNoTrivia);
                rootNode = rootNode.ReplaceNode(rootNode
                    .DescendantNodes()
                    .OfType<ClassDeclarationSyntax>()
                    .SingleOrDefault(n => n.Identifier.ValueText == permissibleName), classNode);
                foreignTree = SyntaxTree(rootNode).WithFilePath(foreignTree.FilePath);

                // Запись и компиляция
                WriteTree(GSCrmApplication, permissibleName, "Table", foreignTree);
                Compile("GSCrmTable", foreignTree);
            }
        }
    }
}