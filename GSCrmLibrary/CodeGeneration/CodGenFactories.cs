using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxKind;
using GSCrmLibrary.Data;
using GSCrmLibrary.Models.TableModels;
using static GSCrmLibrary.CodeGeneration.CodGenUtils;
using static GSCrmLibrary.CodeGeneration.CodGenTreesUtils;
using static GSCrmLibrary.Configuration.ApplicationConfig;
using static GSCrmLibrary.Configuration.EntitiesConfig;
using static GSCrmLibrary.Utils;

namespace GSCrmLibrary.CodeGeneration
{
    public static class CodGenFactories
    {
        public static void GenerateFactory(MainContext context, string factoryType, Guid recordId)
        {
            switch (factoryType)
            {
                case DataFR:
                    var anonymousBC = context.BusinessComponents
                        .AsNoTracking()
                        .Select(bc => new
                        {
                            id = bc.Id,
                            name = bc.Name,
                            tableId = bc.TableId,
                            table = bc.Table == null ? null : new
                            {
                                id = bc.Table.Id,
                                name = bc.Table.Name
                            },
                            fields = bc.Fields.Select(f => new
                            {
                                id = f.Id,
                                name = f.Name,
                                readolny = f.Readonly,
                                isCalculate = f.IsCalculate,
                                calculatedValue = f.CalculatedValue,
                                joinColumnId = f.JoinColumnId,
                                joinColumn = f.JoinColumn == null ? null : new
                                {
                                    id = f.JoinColumn.Id,
                                    name = f.JoinColumn.Name
                                },
                                tableColumnId = f.TableColumnId,
                                tableColumn = f.TableColumn == null ? null : new
                                {
                                    id = f.TableColumn.Id,
                                    name = f.TableColumn.Name
                                },
                                joinId = f.JoinId,
                                join = f.Join == null ? null : new
                                {
                                    id = f.Join.Id,
                                    name = f.Join.Name,
                                    joinSpecifications = f.Join.JoinSpecifications.Select(js => new
                                    {
                                        id = js.Id,
                                        name = js.Name,
                                        sourceFieldId = js.SourceFieldId,
                                        sourceField = js.SourceField == null ? null : new
                                        {
                                            id = js.SourceField.Id,
                                            name = js.SourceField.Name
                                        },
                                        destinationColumnId = js.DestinationColumnId,
                                        destinationColumn = js.DestinationColumn == null ? null : new
                                        {
                                            id = js.DestinationColumn.Id,
                                            name = js.DestinationColumn.Name
                                        }
                                    }),
                                    tableId = f.Join.TableId,
                                    table = f.Join.Table == null ? null : new
                                    {
                                        id = f.Join.Table.Id,
                                        name = f.Join.Table.Name,
                                        tableColumns = f.Join.Table.TableColumns.Select(tc => new
                                        {
                                            id = tc.Id,
                                            name = tc.Name
                                        })
                                    }
                                }
                            })
                        })
                        .FirstOrDefault(i => i.id == recordId);
                        BusinessComponent businessComponent = new BusinessComponent
                        {
                            Id = anonymousBC.id,
                            Name = anonymousBC.name,
                            TableId = anonymousBC.tableId,
                            Table = anonymousBC.table == null ? null : new Table
                            {
                                Id = anonymousBC.table.id,
                                Name = anonymousBC.table.name
                            },
                            Fields = anonymousBC.fields.Select(f => new Field
                            {
                                Id = f.id,
                                Name = f.name,
                                Readonly = f.readolny,
                                IsCalculate = f.isCalculate,
                                CalculatedValue = f.calculatedValue,
                                JoinColumnId = f.joinColumnId,
                                JoinColumn = f.joinColumn == null ? null : new TableColumn
                                {
                                    Id = f.joinColumn.id,
                                    Name = f.joinColumn.name
                                },
                                TableColumnId = f.tableColumnId,
                                TableColumn = f.tableColumn == null ? null : new TableColumn
                                {
                                    Id = f.tableColumn.id,
                                    Name = f.tableColumn.name
                                },
                                JoinId = f.joinId,
                                Join = f.join == null ? null : new Join
                                {
                                    Id = f.join.id,
                                    Name = f.join.name,
                                    JoinSpecifications = f.join.joinSpecifications.Select(js => new JoinSpecification
                                    {
                                        Id = js.id,
                                        Name = js.name,
                                        SourceFieldId = js.sourceFieldId,
                                        SourceField = js.sourceField == null ? null : new Field
                                        {
                                            Id = js.sourceField.id,
                                            Name = js.sourceField.name
                                        },
                                        DestinationColumnId = js.destinationColumnId,
                                        DestinationColumn = js.destinationColumn == null ? null : new TableColumn
                                        {
                                            Id = js.destinationColumn.id,
                                            Name = js.destinationColumn.name
                                        }
                                    }).ToList(),
                                    TableId = f.join.tableId,
                                    Table = f.join.table == null ? null : new Table
                                    {
                                        Id = f.join.table.id,
                                        Name = f.join.table.name,
                                        TableColumns = f.join.table.tableColumns.Select(tc => new TableColumn
                                        {
                                            Id = tc.id,
                                            Name = tc.name
                                        }).ToList()
                                    }
                                }
                            }).ToList()
                        };
                    CreateFactory(true, businessComponent, null, context);
                    break;
                case UIFR:
                    Applet applet = context.Applets
                        .AsNoTracking()
                        .Select(a => new
                        {
                            id = a.Id,
                            name = a.Name,
                            busCompId = a.BusCompId,
                            busComp = a.BusComp == null ? null : new
                            {
                                id = a.BusComp.Id,
                                name = a.BusComp.Name
                            },
                            controls = a.Controls.Select(c => new
                            {
                                id = c.Id,
                                name = c.Name,
                                type = c.Type,
                                @readonly = c.Readonly,
                                fieldId = c.FieldId,
                                field = c.Field == null ? null : new
                                {
                                    id = c.Field.Id,
                                    name = c.Field.Name,
                                    @readonly = c.Field.Readonly
                                }
                            }),
                            columns = a.Columns.Select(c => new
                            {
                                id = c.Id,
                                name = c.Name,
                                type = c.Type,
                                @readonly = c.Readonly,
                                fieldId = c.FieldId,
                                field = c.Field == null ? null : new
                                {
                                    id = c.Field.Id,
                                    name = c.Field.Name,
                                    @readonly = c.Field.Readonly
                                }
                            })
                        })
                        .Select(a => new Applet
                        {
                            Id = a.id,
                            Name = a.name,
                            BusCompId = a.busCompId,
                            BusComp = a.busComp == null ? null : new BusinessComponent
                            {
                                Id = a.busComp.id,
                                Name = a.busComp.name
                            },
                            Controls = a.controls.Select(c => new Control
                            {
                                Id = c.id,
                                Name = c.name,
                                Type = c.type,
                                Readonly = c.@readonly,
                                FieldId = c.fieldId,
                                Field = c.field == null ? null : new Field
                                {
                                    Id = c.field.id,
                                    Name = c.field.name,
                                    Readonly = c.field.@readonly
                                }
                            }).ToList(),
                            Columns = a.columns.Select(c => new Column
                            {
                                Id = c.id,
                                Name = c.name,
                                Type = c.type,
                                Readonly = c.@readonly,
                                FieldId = c.fieldId,
                                Field = c.field == null ? null : new Field
                                {
                                    Id = c.field.id,
                                    Name = c.field.name,
                                    Readonly = c.field.@readonly
                                }
                            }).ToList()
                        })
                        .FirstOrDefault(i => i.Id == recordId);
                    CreateFactory(false, applet.BusComp, applet, context);
                    break;
            }
        }

        public static void GenerateLinksForApplets(MainContext context, Guid recordId)
        {
            View view = context.Views
                .AsNoTracking()
                .Include(vi => vi.ViewItems)
                    .ThenInclude(a => a.Applet)
                .FirstOrDefault(i => i.Id == recordId);

            // Для всех элементво представления, включая попапы, необходимо добавить связи с родительскими сущностями
            view.ViewItems.ForEach(viewItem =>
            {
                Applet applet = context.Applets
                    .AsNoTracking()
                    .Include(cntr => cntr.Controls)
                        .ThenInclude(up => up.ControlUPs)
                    .FirstOrDefault(i => i.Id == viewItem.AppletId);
                List<Applet> appletsToUpdate = new List<Applet>() { viewItem.Applet };

                // Получение попапов, запускаемых с этого апплета
                applet.Controls.Where(c => c.ControlUPs != null && c.ControlUPs.FirstOrDefault(n => n.Name == "Applet") != null).ToList().ForEach(control =>
                {
                    ControlUP appletNameUP = control.ControlUPs.FirstOrDefault(n => n.Name == "Applet");
                    if (appletNameUP != null)
                        appletsToUpdate.Add(context.Applets.AsNoTracking().FirstOrDefault(i => i.Name == appletNameUP.Value));
                });
                appletsToUpdate.Where(ap => ap != null).ToList().ForEach(appletToUpdate =>
                {
                    string factoryName = UIFR + GetPermissibleName(appletToUpdate.Name) + "FR";
                    string factoryType = UIFR + "Factory";
                    BusinessObject businessObject = context.BusinessObjects
                        .AsNoTracking()
                        .Include(boc => boc.BusObjectComponents)
                            .ThenInclude(l => l.Link)
                        .FirstOrDefault(i => i.Id == view.BusObjectId);
                    BusinessObjectComponent childComponent = businessObject.BusObjectComponents.FirstOrDefault(i => i.BusCompId == appletToUpdate.BusCompId);

                    // Если у бизнес компоненты апплета есть родительская бк в объекте, необходимо в фабрику апплета добавить ограничение по родительской сущности
                    if (childComponent.Link != null)
                    {
                        // Если фабрика для этого апплета уже сгенерирована
                        if (TryGetModel(GSCrmApplication, factoryName, factoryType, out SyntaxTree factoryTree))
                        {
                            BusinessObjectComponent parentComponent = businessObject.BusObjectComponents.FirstOrDefault(i => i.BusCompId == childComponent.Link.ParentBCId);
                            BusinessComponent parentBusComp = context.BusinessComponents
                                .AsNoTracking()
                                .Include(t => t.Table)
                                .Include(f => f.Fields)
                                .FirstOrDefault(i => i.Id == parentComponent.BusCompId);
                            BusinessComponent childBusComp = context.BusinessComponents
                                .AsNoTracking()
                                .Include(t => t.Table)
                                .Include(f => f.Fields)
                                .FirstOrDefault(i => i.Id == childComponent.BusCompId);
                            Field parentField = parentBusComp.Fields.FirstOrDefault(i => i.Id == childComponent.Link.ParentFieldId);
                            Field childField = childBusComp.Fields.FirstOrDefault(i => i.Id == childComponent.Link.ChildFieldId);

                            SyntaxNode rootNode = factoryTree.GetRoot();
                            MethodDeclarationSyntax methodDeclaration = rootNode
                                .DescendantNodes()
                                .OfType<MethodDeclarationSyntax>()
                                .FirstOrDefault(n => n.Identifier.ValueText == "UIToBusiness");
                            ExpressionStatementSyntax statement = ExpressionStatement(
                                AssignmentExpression(
                                    SimpleAssignmentExpression,
                                    MemberAccessExpression(
                                        SimpleMemberAccessExpression,
                                        IdentifierName("businessEntity"),
                                        Token(DotToken),
                                        IdentifierName(childField.Name)),
                                    Token(EqualsToken)
                                        .WithLeadingTrivia(Whitespace(" "))
                                        .WithTrailingTrivia(Whitespace(" ")),
                                    InvocationExpression(
                                        MemberAccessExpression(
                                            SimpleMemberAccessExpression,
                                            IdentifierName("Guid"),
                                            Token(DotToken),
                                            IdentifierName("Parse")),
                                        ArgumentList(
                                            Token(OpenParenToken),
                                            SeparatedList(
                                                new List<ArgumentSyntax>()
                                                {
                                                    Argument(
                                                        InvocationExpression(
                                                            MemberAccessExpression(
                                                                SimpleMemberAccessExpression,
                                                                IdentifierName("ComponentsRecordsInfo"),
                                                                Token(DotToken),
                                                                IdentifierName("GetSelectedRecord")),
                                                            ArgumentList(
                                                                Token(OpenParenToken),
                                                                SeparatedList(
                                                                    new List<ArgumentSyntax>()
                                                                    {
                                                                        Argument(LiteralExpression(StringLiteralExpression, ParseToken($"\"{parentBusComp.Name}\"")))
                                                                    }),
                                                                Token(CloseParenToken))))
                                                }),
                                            Token(CloseParenToken)))))
                                .WithLeadingTrivia(GenerateTabs(3))
                                .WithTrailingTrivia(LineFeed);

                            // Получение экземпляра родительской или текущей(при отсутствии родительской) бизнес компоненты
                            if (!methodDeclaration.Body.Statements.ElementAtOrDefault(1).ToFullString().Contains($"ComponentsRecordsInfo.GetSelectedRecord(\"{parentBusComp.Name}\")"))
                            {
                                rootNode = rootNode.InsertNodesAfter(
                                    methodDeclaration.Body.Statements.ElementAtOrDefault(0),
                                    new SyntaxNode[] { statement });
                            }

                            SyntaxTree syntaxTree = SyntaxTree(rootNode).WithFilePath(factoryTree.FilePath);

                            // Запись и компиляция
                            WriteTree(GSCrmApplication, factoryName, factoryType, syntaxTree);
                            Compile("GSCrm" + factoryType, syntaxTree);
                        }
                    }
                });

            });
        }

        private static void CreateFactory(bool isData, BusinessComponent businessComponent, Applet applet, MainContext context)
        {
            // Названия для фабрики и типов, передаваемых в нее
            string firstEntityName = isData ? businessComponent.Table.Name : businessComponent.Name;
            string secondEntityName = isData ? businessComponent.Name : applet.Name;
            string permissibleFirstEntityName = GetPermissibleName(firstEntityName);
            string permissibleSecondEntityName = GetPermissibleName(secondEntityName);
            string sysFirstEntityName = SystemPrefix + permissibleFirstEntityName;
            string sysSecondEntityName = SystemPrefix + permissibleSecondEntityName;
            if (sysFirstEntityName == sysSecondEntityName)
            {
                sysFirstEntityName += "_1";
                sysSecondEntityName += "_2";
            }
            string factoryPrefix = isData ? DataFR : UIFR;
            string factoryName = (isData ? DataFR : UIFR) + permissibleSecondEntityName + "FR";
            string factoryType = factoryPrefix + "Factory";
            string factoryPath = GetEntityExPath(GSCrmApplication, factoryName, factoryType);

            SyntaxTree syntaxTree = SyntaxTree(
                CompilationUnit()
                    .AddUsings(
                        UsingDirective(
                            ParseName("System").WithLeadingTrivia(Whitespace(" "))
                        ).WithTrailingTrivia(LineFeed),
                        UsingDirective(
                            ParseName("System.Linq").WithLeadingTrivia(Whitespace(" "))
                        ).WithTrailingTrivia(LineFeed),
                        UsingDirective(
                            ParseName("System.Collections.Generic").WithLeadingTrivia(Whitespace(" "))
                        ).WithTrailingTrivia(LineFeed),
                        UsingDirective(
                            ParseName("System.ComponentModel.DataAnnotations").WithLeadingTrivia(Whitespace(" "))
                        ).WithTrailingTrivia(LineFeed),
                        UsingDirective(
                            ParseName("Microsoft.EntityFrameworkCore").WithLeadingTrivia(Whitespace(" "))
                        ).WithTrailingTrivia(LineFeed),
                        UsingDirective(
                            ParseName("GSCrmApplication.Data").WithLeadingTrivia(Whitespace(" "))
                        ).WithTrailingTrivia(LineFeed),
                        UsingDirective(
                            ParseName("GSCrmLibrary.Models.MainEntities").WithLeadingTrivia(Whitespace(" "))
                        ).WithTrailingTrivia(LineFeed),
                        UsingDirective(
                            ParseName("GSCrmLibrary.Models.AppletModels").WithLeadingTrivia(Whitespace(" "))
                        ).WithTrailingTrivia(LineFeed),
                        UsingDirective(
                            ParseName("GSCrmLibrary.Models.TableModels").WithLeadingTrivia(Whitespace(" "))
                        ).WithTrailingTrivia(LineFeed),
                        UsingDirective(
                            ParseName("GSCrmApplication.Factories.MainFactories").WithLeadingTrivia(Whitespace(" "))
                        ).WithTrailingTrivia(LineFeed),
                        UsingDirective(
                            ParseName("GSCrmApplication.Models.TableModels").WithLeadingTrivia(Whitespace(" "))
                        ).WithTrailingTrivia(LineFeed),
                        UsingDirective(
                            ParseName("GSCrmLibrary.Services.Info").WithLeadingTrivia(Whitespace(" "))
                        ).WithTrailingTrivia(LineFeed),
                        UsingDirective(
                            ParseName("GSCrmLibrary.Data").WithLeadingTrivia(Whitespace(" "))
                        ).WithTrailingTrivia(LineFeed),
                        UsingDirective(
                            ParseName(isData ? sysFirstEntityName + " = GSCrmApplication.Models.TableModels." + permissibleFirstEntityName :
                            sysFirstEntityName + " = GSCrmApplication.Models.BusinessComponentModels." + permissibleFirstEntityName)
                                .WithLeadingTrivia(Whitespace(" "))
                        ).WithTrailingTrivia(LineFeed),
                        UsingDirective(
                            ParseName(isData ? sysSecondEntityName + " = GSCrmApplication.Models.BusinessComponentModels." + permissibleSecondEntityName :
                            sysSecondEntityName + " = GSCrmApplication.Models.AppletModels." + permissibleSecondEntityName)
                                .WithLeadingTrivia(Whitespace(" "))
                        ).WithTrailingTrivia(Whitespace("\n\n")))
                    .AddMembers(
                        NamespaceDeclaration(
                            QualifiedName(
                                QualifiedName(
                                    IdentifierName("GSCrmApplication"),
                                    Token(DotToken),
                                    IdentifierName("Factories")),
                                Token(DotToken),
                                IdentifierName(factoryPrefix + "Factories")).WithTrailingTrivia(Whitespace("\n")))
                            .WithNamespaceKeyword(Token(NamespaceKeyword).WithTrailingTrivia(Whitespace(" ")))
                            .WithMembers(List(
                                new List<MemberDeclarationSyntax>()
                                {
                                    ClassDeclaration(ParseToken(factoryName).WithTrailingTrivia(Whitespace(" ")))
                                        .WithModifiers(
                                            TokenList(
                                                Token(PublicKeyword)
                                                    .WithLeadingTrivia(Tab)
                                                    .WithTrailingTrivia(Whitespace(" "))))
                                            .WithKeyword(Token(ClassKeyword).WithTrailingTrivia(Whitespace(" ")))

                                        // Открывающая скобка класса
                                        .WithOpenBraceToken(Token(OpenBraceToken).WithTrailingTrivia(LineFeed))

                                        // Базовая фабрика
                                        .WithBaseList(
                                            BaseList(
                                                SingletonSeparatedList<BaseTypeSyntax>(
                                                    SimpleBaseType(
                                                        GenericName(
                                                            Identifier(GetEntityBaseType(factoryType)),
                                                            TypeArgumentList(
                                                                Token(LessThanToken),
                                                                SeparatedList(
                                                                    new List<TypeSyntax>()
                                                                    {
                                                                        IdentifierName(ParseToken(sysFirstEntityName)),
                                                                        IdentifierName(ParseToken(sysSecondEntityName)).WithLeadingTrivia(Whitespace(" ")),
                                                                        IdentifierName("GSAppContext").WithLeadingTrivia(Whitespace(" "))
                                                                    },
                                                                    GetSeparators(2, CommaToken)),
                                                                Token(GreaterThanToken).WithTrailingTrivia(LineFeed)))
                                                        .WithLeadingTrivia(Whitespace(" "))))))

                                        // Методы фабрики
                                        .WithMembers(List(
                                            isData ? 
                                            new List<MemberDeclarationSyntax>()
                                            {
                                                #region DataToBusiness
                                                MethodDeclaration(ParseTypeName(sysSecondEntityName).WithTrailingTrivia(Whitespace(" ")), "DataToBusiness")

                                                    // Модификатор доступа и ключевое слово override
                                                    .WithModifiers(
                                                        TokenList(
                                                            Token(PublicKeyword)
                                                                .WithTrailingTrivia(Whitespace(" "))
                                                                .WithLeadingTrivia(GenerateTabs(2)),
                                                            Token(OverrideKeyword).WithTrailingTrivia(Whitespace(" "))))
                                                    
                                                    // Список параметров
                                                    .WithParameterList(
                                                        ParameterList()
                                                            .WithOpenParenToken(Token(OpenParenToken))
                                                            .WithParameters(
                                                                SeparatedList(
                                                                    new List<ParameterSyntax>()
                                                                    {
                                                                        Parameter(
                                                                            List(new List<AttributeListSyntax>(){ }),
                                                                            TokenList(),
                                                                            ParseTypeName(sysFirstEntityName).WithTrailingTrivia(Whitespace(" ")),
                                                                            Identifier("dataEntity"),
                                                                            default),
                                                                        Parameter(
                                                                            List(new List<AttributeListSyntax>(){ }),
                                                                            TokenList(),
                                                                            ParseTypeName("GSAppContext").WithTrailingTrivia(Whitespace(" ")),
                                                                            Identifier("context"),
                                                                            default).WithLeadingTrivia(Whitespace(" "))
                                                                    }))
                                                            .WithCloseParenToken(Token(CloseParenToken)).WithTrailingTrivia(LineFeed))

                                                    // Тело метода
                                                    .WithBody(
                                                        Block(List(
                                                            new List<SyntaxNode>()
                                                            {
                                                                // Выполнение метода базовой фабрики
                                                                LocalDeclarationStatement(
                                                                    VariableDeclaration(
                                                                        IdentifierName(sysSecondEntityName)
                                                                            .WithLeadingTrivia(GenerateTabs(3))
                                                                            .WithTrailingTrivia(Whitespace(" ")),
                                                                        SeparatedList(
                                                                            new List<VariableDeclaratorSyntax>()
                                                                            {
                                                                                VariableDeclarator(Identifier("businessEntity").WithTrailingTrivia(Whitespace(" ")))
                                                                                    .WithInitializer(
                                                                                        EqualsValueClause(
                                                                                            Token(EqualsToken).WithTrailingTrivia(Whitespace(" ")),
                                                                                            InvocationExpression(
                                                                                                MemberAccessExpression(
                                                                                                    SimpleMemberAccessExpression,
                                                                                                    BaseExpression(Token(BaseKeyword)),
                                                                                                    Token(DotToken),
                                                                                                    IdentifierName("DataToBusiness")),
                                                                                                ArgumentList(
                                                                                                    Token(OpenParenToken),
                                                                                                    SeparatedList(
                                                                                                        new List<ArgumentSyntax>()
                                                                                                        {
                                                                                                            // Сущность уровня данных
                                                                                                            Argument(IdentifierName("dataEntity")),

                                                                                                            // Контекст приложения
                                                                                                            Argument(IdentifierName("context").WithLeadingTrivia(Whitespace(" ")))
                                                                                                        },
                                                                                                        GetSeparators(1, CommaToken)),
                                                                                                    Token(CloseParenToken)))))
                                                                            })))
                                                                .WithTrailingTrivia(LineFeed)
                                                            }

                                                            // Поля, основанные на реальных колонках
                                                            .Concat(MapRealColumnsToFields(businessComponent.Fields))
                                                            
                                                            // Поля основанные на join-ах
                                                            .Concat(MapJoinColumnsToFields(context, businessComponent, businessComponent.Fields))

                                                            // Калькулируемые поля
                                                            .Concat(InitializeCalcFields(businessComponent.Fields))
                                                            
                                                            // Вовзращаемое значение
                                                            .Concat(new SyntaxNode[]
                                                            {
                                                                ReturnStatement(
                                                                    Token(ReturnKeyword)
                                                                        .WithLeadingTrivia(GenerateTabs(3))
                                                                        .WithTrailingTrivia(Whitespace(" ")),
                                                                    IdentifierName("businessEntity"),
                                                                    Token(SemicolonToken))
                                                                .WithTrailingTrivia(LineFeed)
                                                            })))

                                                    // Открывающая фигурная скобка метода
                                                    .WithOpenBraceToken(Token(
                                                        TriviaList(
                                                            GenerateTabs(2)),
                                                            OpenBraceToken,
                                                            TriviaList(LineFeed)))

                                                    // Закрывающая фигурная скобка метода
                                                    .WithCloseBraceToken(Token(
                                                        TriviaList(
                                                            GenerateTabs(2)),
                                                            CloseBraceToken,
                                                            TriviaList())).WithTrailingTrivia(LineFeed)),
                                                #endregion

                                                #region BusinessToData 
                                                MethodDeclaration(ParseTypeName(sysFirstEntityName).WithTrailingTrivia(Whitespace(" ")), "BusinessToData")
                                                        
                                                    // Модификатор доступа и ключевое слово override
                                                    .WithModifiers(
                                                        TokenList(
                                                            Token(PublicKeyword)
                                                                .WithTrailingTrivia(Whitespace(" "))
                                                                .WithLeadingTrivia(GenerateTabs(2)),
                                                            Token(OverrideKeyword).WithTrailingTrivia(Whitespace(" "))))

                                                    // Список параметров
                                                    .WithParameterList(
                                                        ParameterList()
                                                            .WithOpenParenToken(Token(OpenParenToken))
                                                            .WithParameters(
                                                                SeparatedList(
                                                                    new List<ParameterSyntax>()
                                                                    {
                                                                        Parameter(
                                                                            List(new List<AttributeListSyntax>(){ }),
                                                                            TokenList(),
                                                                            ParseTypeName(sysFirstEntityName).WithTrailingTrivia(Whitespace(" ")),
                                                                            Identifier(sysFirstEntityName.ToLower()),
                                                                            default),
                                                                        Parameter(
                                                                            List(new List<AttributeListSyntax>(){ }),
                                                                            TokenList(),
                                                                            ParseTypeName(sysSecondEntityName).WithTrailingTrivia(Whitespace(" ")),
                                                                            Identifier("businessEntity"),
                                                                            default).WithLeadingTrivia(Whitespace(" ")),
                                                                        Parameter(
                                                                            List(new List<AttributeListSyntax>(){ }),
                                                                            TokenList(),
                                                                            ParseTypeName("GSAppContext").WithTrailingTrivia(Whitespace(" ")),
                                                                            Identifier("context"),
                                                                            default).WithLeadingTrivia(Whitespace(" ")),
                                                                        Parameter(
                                                                            List(new List<AttributeListSyntax>(){ }),
                                                                            TokenList(),
                                                                            ParseTypeName("bool").WithTrailingTrivia(Whitespace(" ")),
                                                                            Identifier("NewRecord"),
                                                                            default).WithLeadingTrivia(Whitespace(" "))
                                                                    }))
                                                            .WithCloseParenToken(Token(CloseParenToken)).WithTrailingTrivia(LineFeed))

                                                    // Тело метода
                                                    .WithBody(
                                                        Block(List(
                                                            new List<StatementSyntax>()
                                                            {
                                                                // Выполнение метода базовой фабрики
                                                                LocalDeclarationStatement(
                                                                    VariableDeclaration(
                                                                        IdentifierName(sysFirstEntityName)
                                                                            .WithLeadingTrivia(GenerateTabs(3))
                                                                            .WithTrailingTrivia(Whitespace(" ")),
                                                                        SeparatedList(
                                                                            new List<VariableDeclaratorSyntax>()
                                                                            {
                                                                                VariableDeclarator(Identifier("dataEntity").WithTrailingTrivia(Whitespace(" ")))
                                                                                    .WithInitializer(
                                                                                        EqualsValueClause(
                                                                                            Token(EqualsToken).WithTrailingTrivia(Whitespace(" ")),
                                                                                            InvocationExpression(
                                                                                                MemberAccessExpression(
                                                                                                    SimpleMemberAccessExpression,
                                                                                                    BaseExpression(Token(BaseKeyword)),
                                                                                                    Token(DotToken),
                                                                                                    IdentifierName("BusinessToData")),
                                                                                                ArgumentList(
                                                                                                    Token(OpenParenToken),
                                                                                                    SeparatedList(
                                                                                                        new List<ArgumentSyntax>()
                                                                                                        {
                                                                                                            // Запись для маппинга
                                                                                                            Argument(IdentifierName(sysFirstEntityName.ToLower())),

                                                                                                            // Сущность бизнес уровня
                                                                                                            Argument(IdentifierName("businessEntity").WithLeadingTrivia(Whitespace(" "))),

                                                                                                            // Контекст
                                                                                                            Argument(IdentifierName("context").WithLeadingTrivia(Whitespace(" "))),

                                                                                                            // Признак, является ли запись новой
                                                                                                            Argument(IdentifierName("NewRecord").WithLeadingTrivia(Whitespace(" ")))
                                                                                                        },
                                                                                                        GetSeparators(3, CommaToken)),
                                                                                                    Token(CloseParenToken)))))
                                                                            })))
                                                                .WithTrailingTrivia(LineFeed)
                                                            }
                                                            
                                                            // Маппинг полей бизнес компоненты в колонки таблицы
                                                            .Concat(MapFieldsToTableColumns(businessComponent.Fields))
                                                            
                                                            // Вовзращаемое значение
                                                            .Concat(new SyntaxNode[]
                                                            {
                                                                ReturnStatement(
                                                                    Token(ReturnKeyword)
                                                                        .WithLeadingTrivia(GenerateTabs(3))
                                                                        .WithTrailingTrivia(Whitespace(" ")),
                                                                    IdentifierName("dataEntity"),
                                                                    Token(SemicolonToken))
                                                                .WithTrailingTrivia(LineFeed)
                                                            })))

                                                    // Открывающая фигурная скобка метода
                                                    .WithOpenBraceToken(Token(
                                                        TriviaList(
                                                            GenerateTabs(2)),
                                                            OpenBraceToken,
                                                            TriviaList(LineFeed)))

                                                    // Закрывающая фигурная скобка метода
                                                    .WithCloseBraceToken(Token(
                                                        TriviaList(
                                                            GenerateTabs(2)),
                                                            CloseBraceToken,
                                                            TriviaList())).WithTrailingTrivia(LineFeed)),
                                                #endregion

                                                #region DataBUSValidate
                                                MethodDeclaration(ParseTypeName("IEnumerable<ValidationResult>").WithTrailingTrivia(Whitespace(" ")), "DataBUSValidate")

                                                    // Модификатор доступа и ключевое слово override
                                                    .WithModifiers(
                                                        TokenList(
                                                            Token(PublicKeyword)
                                                                .WithTrailingTrivia(Whitespace(" "))
                                                                .WithLeadingTrivia(Whitespace("\n        ")),
                                                            Token(OverrideKeyword).WithTrailingTrivia(Whitespace(" "))))
                                                                                                        
                                                    // Список параметров
                                                    .WithParameterList(
                                                        ParameterList()
                                                            .WithOpenParenToken(Token(OpenParenToken))
                                                            .WithParameters(
                                                                SeparatedList(
                                                                    new List<ParameterSyntax>()
                                                                    {
                                                                        Parameter(
                                                                            List(new List<AttributeListSyntax>(){ }),
                                                                            TokenList(),
                                                                            ParseTypeName(sysFirstEntityName).WithTrailingTrivia(Whitespace(" ")),
                                                                            Identifier("dataEntity"),
                                                                            default),
                                                                        Parameter(
                                                                            List(new List<AttributeListSyntax>(){ }),
                                                                            TokenList(),
                                                                            ParseTypeName(sysSecondEntityName)
                                                                                .WithLeadingTrivia(Whitespace(" "))
                                                                                .WithTrailingTrivia(Whitespace(" ")),
                                                                            Identifier("businessEntity"),
                                                                            default),
                                                                        Parameter(
                                                                            List(new List<AttributeListSyntax>(){ }),
                                                                            TokenList(),
                                                                            ParseTypeName("IViewInfo")
                                                                                .WithLeadingTrivia(Whitespace(" "))
                                                                                .WithTrailingTrivia(Whitespace(" ")),
                                                                            Identifier("viewInfo"),
                                                                            default),
                                                                        Parameter(
                                                                            List(new List<AttributeListSyntax>(){ }),
                                                                            TokenList(),
                                                                            ParseTypeName("GSAppContext")
                                                                                .WithLeadingTrivia(Whitespace(" "))
                                                                                .WithTrailingTrivia(Whitespace(" ")),
                                                                            Identifier("context"),
                                                                            default)
                                                                    },
                                                                    GetSeparators(3, CommaToken)))
                                                            .WithCloseParenToken(Token(CloseParenToken).WithTrailingTrivia(LineFeed)))

                                                    // Тело 
                                                    .WithBody(
                                                        Block(List(
                                                            new List<StatementSyntax>()
                                                            {
                                                                // Вызов метода из родительской фабрики
                                                                LocalDeclarationStatement(
                                                                    VariableDeclaration(
                                                                        GenericName(
                                                                            Identifier("List").WithLeadingTrivia(GenerateTabs(3)),
                                                                            TypeArgumentList(
                                                                                Token(LessThanToken),
                                                                                SeparatedList(new List<TypeSyntax>() { IdentifierName("ValidationResult") }),
                                                                                Token(GreaterThanToken).WithTrailingTrivia(Whitespace(" ")))),
                                                                        SeparatedList(
                                                                            new List<VariableDeclaratorSyntax>()
                                                                            {
                                                                                VariableDeclarator(Identifier("result").WithTrailingTrivia(Whitespace(" ")))
                                                                                    .WithInitializer(
                                                                                        EqualsValueClause(
                                                                                            Token(EqualsToken).WithTrailingTrivia(Whitespace(" ")),
                                                                                            InvocationExpression(
                                                                                                MemberAccessExpression(
                                                                                                    SimpleMemberAccessExpression,
                                                                                                    InvocationExpression(
                                                                                                        MemberAccessExpression(
                                                                                                            SimpleMemberAccessExpression,
                                                                                                            BaseExpression(Token(BaseKeyword)),
                                                                                                            Token(DotToken),
                                                                                                            IdentifierName("DataBUSValidate")),
                                                                                                        ArgumentList(
                                                                                                            Token(OpenParenToken),
                                                                                                            SeparatedList(
                                                                                                                new List<ArgumentSyntax>()
                                                                                                                {
                                                                                                                    Argument(IdentifierName("dataEntity")),
                                                                                                                    Argument(IdentifierName("businessEntity")).WithLeadingTrivia(Whitespace(" ")),
                                                                                                                    Argument(IdentifierName("viewInfo")).WithLeadingTrivia(Whitespace(" ")),
                                                                                                                    Argument(IdentifierName("context")).WithLeadingTrivia(Whitespace(" "))
                                                                                                                },
                                                                                                                GetSeparators(3, CommaToken)),
                                                                                                            Token(CloseParenToken))),
                                                                                                    Token(DotToken),
                                                                                                    IdentifierName("ToList")),
                                                                                                ArgumentList(
                                                                                                    Token(OpenParenToken),
                                                                                                    SeparatedList(new List<ArgumentSyntax>(){ }),
                                                                                                    Token(CloseParenToken)))))
                                                                            })))
                                                                .WithTrailingTrivia(LineFeed),

                                                                // Получение текущего апплета
                                                                LocalDeclarationStatement(
                                                                    VariableDeclaration(
                                                                        IdentifierName("Applet")
                                                                            .WithLeadingTrivia(GenerateTabs(3))
                                                                            .WithTrailingTrivia(Whitespace(" ")),
                                                                        SeparatedList(
                                                                            new List<VariableDeclaratorSyntax>()
                                                                            {
                                                                                VariableDeclarator(Identifier("currentApplet").WithTrailingTrivia(Whitespace(" ")))
                                                                                    .WithInitializer(
                                                                                        EqualsValueClause(
                                                                                            Token(EqualsToken).WithTrailingTrivia(Whitespace(" ")),
                                                                                            BinaryExpression(
                                                                                                CoalesceExpression,
                                                                                                MemberAccessExpression(
                                                                                                    SimpleMemberAccessExpression,
                                                                                                    IdentifierName("viewInfo"),
                                                                                                    Token(DotToken),
                                                                                                    IdentifierName("CurrentPopupApplet").WithTrailingTrivia(Whitespace(" "))
                                                                                                ),
                                                                                                Token(QuestionQuestionToken).WithTrailingTrivia(Whitespace(" ")),
                                                                                                MemberAccessExpression(
                                                                                                    SimpleMemberAccessExpression,
                                                                                                    IdentifierName("viewInfo"),
                                                                                                    Token(DotToken),
                                                                                                    IdentifierName("CurrentApplet")))))
                                                                            })))
                                                                .WithTrailingTrivia(LineFeed)
                                                            }

                                                            // Проверки полей на ридонли
                                                            .Concat(GetReadonlyChecks(businessComponent))

                                                            // Вовзращаемое значение
                                                            .Concat(new SyntaxNode[]
                                                            {
                                                                ReturnStatement(
                                                                    Token(ReturnKeyword)
                                                                        .WithLeadingTrivia(GenerateTabs(3))
                                                                        .WithTrailingTrivia(Whitespace(" ")),
                                                                    IdentifierName("result"),
                                                                    Token(SemicolonToken))
                                                                .WithTrailingTrivia(LineFeed)
                                                            })))

                                                        // Открывающая фигурная скобка метода
                                                        .WithOpenBraceToken(Token(
                                                            TriviaList(
                                                                GenerateTabs(2)),
                                                                OpenBraceToken,
                                                                TriviaList(LineFeed)))

                                                        // Закрывающая фигурная скобка метода
                                                        .WithCloseBraceToken(Token(
                                                            TriviaList(
                                                                GenerateTabs(2)),
                                                                CloseBraceToken,
                                                                TriviaList()))
                                                            .WithTrailingTrivia(LineFeed))
                                                #endregion
                                            }
                                            :
                                            new List<MemberDeclarationSyntax>()
                                            {
                                                #region UIToBusiness
                                                MethodDeclaration(ParseTypeName(sysFirstEntityName).WithTrailingTrivia(Whitespace(" ")), "UIToBusiness")

                                                    // Модификатор доступа и ключевое слово override
                                                    .WithModifiers(
                                                        TokenList(
                                                            Token(PublicKeyword)
                                                                .WithTrailingTrivia(Whitespace(" "))
                                                                .WithLeadingTrivia(GenerateTabs(2)),
                                                            Token(OverrideKeyword).WithTrailingTrivia(Whitespace(" "))))
     
                                                    // Список параметров
                                                    .WithParameterList(
                                                        ParameterList()
                                                            .WithOpenParenToken(Token(OpenParenToken))
                                                            .WithParameters(
                                                                SeparatedList(
                                                                    new List<ParameterSyntax>()
                                                                    {
                                                                        Parameter(
                                                                            List(new List<AttributeListSyntax>(){ }),
                                                                            TokenList(),
                                                                            ParseTypeName(sysSecondEntityName).WithTrailingTrivia(Whitespace(" ")),
                                                                            Identifier("UIEntity"),
                                                                            default),
                                                                        Parameter(
                                                                            List(new List<AttributeListSyntax>(){ }),
                                                                            TokenList(),
                                                                            ParseTypeName("GSAppContext").WithTrailingTrivia(Whitespace(" ")),
                                                                            Identifier("context"),
                                                                            default).WithLeadingTrivia(Whitespace(" ")),
                                                                        Parameter(
                                                                            List(new List<AttributeListSyntax>(){ }),
                                                                            TokenList(),
                                                                            ParseTypeName("IViewInfo").WithTrailingTrivia(Whitespace(" ")),
                                                                            Identifier("viewInfo"),
                                                                            default).WithLeadingTrivia(Whitespace(" ")),
                                                                        Parameter(
                                                                            List(new List<AttributeListSyntax>(){ }),
                                                                            TokenList(),
                                                                            ParseTypeName("bool").WithTrailingTrivia(Whitespace(" ")),
                                                                            Identifier("NewRecord"),
                                                                            default).WithLeadingTrivia(Whitespace(" "))
                                                                    }))
                                                            .WithCloseParenToken(Token(CloseParenToken)).WithTrailingTrivia(LineFeed))

                                                    // Тело метода
                                                    .WithBody(
                                                        Block(List(
                                                            new List<StatementSyntax>()
                                                            {
                                                                // Выполнение метода базовой фабрики
                                                                LocalDeclarationStatement(
                                                                    VariableDeclaration(
                                                                        IdentifierName(sysFirstEntityName)
                                                                            .WithLeadingTrivia(GenerateTabs(3))
                                                                            .WithTrailingTrivia(Whitespace(" ")),
                                                                        SeparatedList(
                                                                            new List<VariableDeclaratorSyntax>()
                                                                            {
                                                                                VariableDeclarator(Identifier("businessEntity").WithTrailingTrivia(Whitespace(" ")))
                                                                                    .WithInitializer(
                                                                                        EqualsValueClause(
                                                                                            Token(EqualsToken).WithTrailingTrivia(Whitespace(" ")),
                                                                                            InvocationExpression(
                                                                                                MemberAccessExpression(
                                                                                                    SimpleMemberAccessExpression,
                                                                                                    BaseExpression(Token(BaseKeyword)),
                                                                                                    Token(DotToken),
                                                                                                    IdentifierName("UIToBusiness")),
                                                                                                ArgumentList(
                                                                                                    Token(OpenParenToken),
                                                                                                    SeparatedList(
                                                                                                        new List<ArgumentSyntax>()
                                                                                                        {
                                                                                                            // Сущность уровня данных
                                                                                                            Argument(IdentifierName("UIEntity")),

                                                                                                            // Контекст приложения
                                                                                                            Argument(IdentifierName("context").WithLeadingTrivia(Whitespace(" "))),

                                                                                                            // Информация о представлении
                                                                                                            Argument(IdentifierName("viewInfo").WithLeadingTrivia(Whitespace(" "))),

                                                                                                            // Признак, является ли запись новой
                                                                                                            Argument(IdentifierName("NewRecord").WithLeadingTrivia(Whitespace(" ")))
                                                                                                        },
                                                                                                        GetSeparators(3, CommaToken)),
                                                                                                    Token(CloseParenToken)))))
                                                                            })))
                                                                .WithTrailingTrivia(LineFeed)
                                                            }

                                                            // Маппинг контролов и колонок
                                                            .Concat(MapControlsToFields(applet.Controls, applet.Columns))

                                                            // Вовзращаемое значение
                                                            .Concat(new SyntaxNode[]
                                                            {
                                                                ReturnStatement(
                                                                    Token(ReturnKeyword)
                                                                        .WithLeadingTrivia(GenerateTabs(3))
                                                                        .WithTrailingTrivia(Whitespace(" ")),
                                                                    IdentifierName("businessEntity"),
                                                                    Token(SemicolonToken))
                                                                .WithTrailingTrivia(LineFeed)
                                                            })))

                                                    // Открывающая фигурная скобка метода
                                                    .WithOpenBraceToken(Token(
                                                        TriviaList(
                                                            GenerateTabs(2)),
                                                            OpenBraceToken,
                                                            TriviaList(LineFeed)))

                                                    // Закрывающая фигурная скобка метода
                                                    .WithCloseBraceToken(Token(
                                                        TriviaList(
                                                            GenerateTabs(2)),
                                                            CloseBraceToken,
                                                            TriviaList()))),
                                                #endregion

                                                #region BusinessToUI
                                                MethodDeclaration(ParseTypeName(sysSecondEntityName).WithTrailingTrivia(Whitespace(" ")), "BusinessToUI")

                                                    // Модификатор доступа и ключевое слово override
                                                    .WithModifiers(
                                                        TokenList(
                                                            Token(PublicKeyword)
                                                                .WithTrailingTrivia(Whitespace(" "))
                                                                .WithLeadingTrivia(Whitespace("\n\n        ")),
                                                            Token(OverrideKeyword).WithTrailingTrivia(Whitespace(" "))))
                                                                                                        
                                                    // Список параметров
                                                    .WithParameterList(
                                                        ParameterList()
                                                            .WithOpenParenToken(Token(OpenParenToken))
                                                            .WithParameters(
                                                                SeparatedList(
                                                                    new List<ParameterSyntax>()
                                                                    {
                                                                        Parameter(
                                                                            List(new List<AttributeListSyntax>(){ }),
                                                                            TokenList(),
                                                                            ParseTypeName(sysFirstEntityName).WithTrailingTrivia(Whitespace(" ")),
                                                                            Identifier("businessEntity"),
                                                                            default)
                                                                    }))
                                                            .WithCloseParenToken(Token(CloseParenToken)).WithTrailingTrivia(LineFeed))
                                                    
                                                    // Тело метода
                                                    .WithBody(
                                                        Block(List(
                                                            new List<StatementSyntax>()
                                                            {
                                                                // Выполнение метода базовой фабрики
                                                                LocalDeclarationStatement(
                                                                    VariableDeclaration(
                                                                        IdentifierName(sysSecondEntityName)
                                                                            .WithLeadingTrivia(GenerateTabs(3))
                                                                            .WithTrailingTrivia(Whitespace(" ")),
                                                                        SeparatedList(
                                                                            new List<VariableDeclaratorSyntax>()
                                                                            {
                                                                                VariableDeclarator(Identifier("UIEntity").WithTrailingTrivia(Whitespace(" ")))
                                                                                    .WithInitializer(
                                                                                        EqualsValueClause(
                                                                                            Token(EqualsToken).WithTrailingTrivia(Whitespace(" ")),
                                                                                            InvocationExpression(
                                                                                                MemberAccessExpression(
                                                                                                    SimpleMemberAccessExpression,
                                                                                                    BaseExpression(Token(BaseKeyword)),
                                                                                                    Token(DotToken),
                                                                                                    IdentifierName("BusinessToUI")),
                                                                                                ArgumentList(
                                                                                                    Token(OpenParenToken),
                                                                                                    SeparatedList(
                                                                                                        new List<ArgumentSyntax>()
                                                                                                        {
                                                                                                            // Сущность бизнес уровня
                                                                                                            Argument(IdentifierName("businessEntity"))
                                                                                                        }),
                                                                                                    Token(CloseParenToken)))))
                                                                            })))
                                                                .WithTrailingTrivia(LineFeed)
                                                            }

                                                            // Маппинг контролов и колонок
                                                            .Concat(MapFieldsToControls(applet.Controls, applet.Columns))

                                                            // Вовзращаемое значение
                                                            .Concat(new SyntaxNode[]
                                                            {
                                                                ReturnStatement(
                                                                    Token(ReturnKeyword)
                                                                        .WithLeadingTrivia(GenerateTabs(3))
                                                                        .WithTrailingTrivia(Whitespace(" ")),
                                                                    IdentifierName("UIEntity"),
                                                                    Token(SemicolonToken))
                                                                .WithTrailingTrivia(LineFeed)
                                                            })))

                                                    // Открывающая фигурная скобка метода
                                                    .WithOpenBraceToken(Token(
                                                        TriviaList(
                                                            GenerateTabs(2)),
                                                            OpenBraceToken,
                                                            TriviaList(LineFeed)))

                                                    // Закрывающая фигурная скобка метода
                                                    .WithCloseBraceToken(Token(
                                                        TriviaList(
                                                            GenerateTabs(2)),
                                                            CloseBraceToken,
                                                            TriviaList())).WithTrailingTrivia(LineFeed)),
                                                #endregion

                                                #region UIValidate
                                                MethodDeclaration(ParseTypeName("IEnumerable<ValidationResult>").WithTrailingTrivia(Whitespace(" ")), "UIValidate")

                                                    // Модификатор доступа и ключевое слово override
                                                    .WithModifiers(
                                                        TokenList(
                                                            Token(PublicKeyword)
                                                                .WithTrailingTrivia(Whitespace(" "))
                                                                .WithLeadingTrivia(Whitespace("\n        ")),
                                                            Token(OverrideKeyword).WithTrailingTrivia(Whitespace(" "))))
  
                                                    // Список параметров
                                                    .WithParameterList(
                                                        ParameterList()
                                                            .WithOpenParenToken(Token(OpenParenToken))
                                                            .WithParameters(
                                                                SeparatedList(
                                                                    new List<ParameterSyntax>()
                                                                    {
                                                                        Parameter(
                                                                            List(new List<AttributeListSyntax>(){ }),
                                                                            TokenList(),
                                                                            ParseTypeName("GSAppContext").WithTrailingTrivia(Whitespace(" ")),
                                                                            Identifier("context"),
                                                                            default),
                                                                        Parameter(
                                                                            List(new List<AttributeListSyntax>(){ }),
                                                                            TokenList(),
                                                                            ParseTypeName("IViewInfo")
                                                                                .WithLeadingTrivia(Whitespace(" "))
                                                                                .WithTrailingTrivia(Whitespace(" ")),
                                                                            Identifier("viewInfo"),
                                                                            default),
                                                                        Parameter(
                                                                            List(new List<AttributeListSyntax>(){ }),
                                                                            TokenList(),
                                                                            ParseTypeName(sysSecondEntityName)
                                                                                .WithLeadingTrivia(Whitespace(" "))
                                                                                .WithTrailingTrivia(Whitespace(" ")),
                                                                            Identifier("UIEntity"),
                                                                            default),
                                                                        Parameter(
                                                                            List(new List<AttributeListSyntax>(){ }),
                                                                            TokenList(),
                                                                            ParseTypeName("bool")
                                                                                .WithLeadingTrivia(Whitespace(" "))
                                                                                .WithTrailingTrivia(Whitespace(" ")),
                                                                            Identifier("isNewRecord"),
                                                                            default)
                                                                    },
                                                                    GetSeparators(3, CommaToken)))
                                                            .WithCloseParenToken(Token(CloseParenToken).WithTrailingTrivia(LineFeed)))

                                                    // Тело 
                                                    .WithBody(
                                                        Block(List(
                                                            new List<StatementSyntax>()
                                                            {
                                                                // Вызов метода из родительской фабрики
                                                                LocalDeclarationStatement(
                                                                    VariableDeclaration(
                                                                        GenericName(
                                                                            Identifier("List").WithLeadingTrivia(GenerateTabs(3)),
                                                                            TypeArgumentList(
                                                                                Token(LessThanToken),
                                                                                SeparatedList(new List<TypeSyntax>() { IdentifierName("ValidationResult") }),
                                                                                Token(GreaterThanToken).WithTrailingTrivia(Whitespace(" ")))),
                                                                        SeparatedList(
                                                                            new List<VariableDeclaratorSyntax>()
                                                                            {
                                                                                VariableDeclarator(Identifier("result").WithTrailingTrivia(Whitespace(" ")))
                                                                                    .WithInitializer(
                                                                                        EqualsValueClause(
                                                                                            Token(EqualsToken).WithTrailingTrivia(Whitespace(" ")),
                                                                                            InvocationExpression(
                                                                                                MemberAccessExpression(
                                                                                                    SimpleMemberAccessExpression,
                                                                                                    InvocationExpression(
                                                                                                        MemberAccessExpression(
                                                                                                            SimpleMemberAccessExpression,
                                                                                                            BaseExpression(Token(BaseKeyword)),
                                                                                                            Token(DotToken),
                                                                                                            IdentifierName("UIValidate")),
                                                                                                        ArgumentList(
                                                                                                            Token(OpenParenToken),
                                                                                                            SeparatedList(
                                                                                                                new List<ArgumentSyntax>()
                                                                                                                {
                                                                                                                    Argument(IdentifierName("context")),
                                                                                                                    Argument(IdentifierName("viewInfo")).WithLeadingTrivia(Whitespace(" ")),
                                                                                                                    Argument(IdentifierName("UIEntity")).WithLeadingTrivia(Whitespace(" ")),
                                                                                                                    Argument(IdentifierName("isNewRecord")).WithLeadingTrivia(Whitespace(" "))
                                                                                                                },
                                                                                                                GetSeparators(3, CommaToken)),
                                                                                                            Token(CloseParenToken))),
                                                                                                    Token(DotToken),
                                                                                                    IdentifierName("ToList")),
                                                                                                ArgumentList(
                                                                                                    Token(OpenParenToken),
                                                                                                    SeparatedList(new List<ArgumentSyntax>(){ }),
                                                                                                    Token(CloseParenToken)))))
                                                                            })))
                                                                .WithTrailingTrivia(LineFeed)
                                                            }

                                                            // Вовзращаемое значение
                                                            .Concat(new SyntaxNode[]
                                                            {
                                                                ReturnStatement(
                                                                    Token(ReturnKeyword)
                                                                        .WithLeadingTrivia(GenerateTabs(3))
                                                                        .WithTrailingTrivia(Whitespace(" ")),
                                                                    IdentifierName("result"),
                                                                    Token(SemicolonToken))
                                                                .WithTrailingTrivia(LineFeed)
                                                            })))

                                                        // Открывающая фигурная скобка метода
                                                        .WithOpenBraceToken(Token(
                                                            TriviaList(
                                                                GenerateTabs(2)),
                                                                OpenBraceToken,
                                                                TriviaList(LineFeed)))

                                                        // Закрывающая фигурная скобка метода
                                                        .WithCloseBraceToken(Token(
                                                            TriviaList(
                                                                GenerateTabs(2)),
                                                                CloseBraceToken,
                                                                TriviaList()))
                                                            .WithTrailingTrivia(LineFeed)),
                                                #endregion

                                                #region BUSUIValidate
                                                MethodDeclaration(ParseTypeName("IEnumerable<ValidationResult>").WithTrailingTrivia(Whitespace(" ")), "BUSUIValidate")

                                                    // Модификатор доступа и ключевое слово override
                                                    .WithModifiers(
                                                        TokenList(
                                                            Token(PublicKeyword)
                                                                .WithTrailingTrivia(Whitespace(" "))
                                                                .WithLeadingTrivia(Whitespace("\n        ")),
                                                            Token(OverrideKeyword).WithTrailingTrivia(Whitespace(" "))))
                                                                                                        
                                                    // Список параметров
                                                    .WithParameterList(
                                                        ParameterList()
                                                            .WithOpenParenToken(Token(OpenParenToken))
                                                            .WithParameters(
                                                                SeparatedList(
                                                                    new List<ParameterSyntax>()
                                                                    {
                                                                        Parameter(
                                                                            List(new List<AttributeListSyntax>(){ }),
                                                                            TokenList(),
                                                                            ParseTypeName("GSAppContext").WithTrailingTrivia(Whitespace(" ")),
                                                                            Identifier("context"),
                                                                            default),
                                                                        Parameter(
                                                                            List(new List<AttributeListSyntax>(){ }),
                                                                            TokenList(),
                                                                            ParseTypeName(sysFirstEntityName)
                                                                                .WithLeadingTrivia(Whitespace(" "))
                                                                                .WithTrailingTrivia(Whitespace(" ")),
                                                                            Identifier("businessEntity"),
                                                                            default),
                                                                        Parameter(
                                                                            List(new List<AttributeListSyntax>(){ }),
                                                                            TokenList(),
                                                                            ParseTypeName(sysSecondEntityName)
                                                                                .WithLeadingTrivia(Whitespace(" "))
                                                                                .WithTrailingTrivia(Whitespace(" ")),
                                                                            Identifier("UIEntity"),
                                                                            default)
                                                                    },
                                                                    GetSeparators(2, CommaToken)))
                                                            .WithCloseParenToken(Token(CloseParenToken).WithTrailingTrivia(LineFeed)))

                                                    // Тело 
                                                    .WithBody(
                                                        Block(List(
                                                            new List<StatementSyntax>()
                                                            {
                                                                LocalDeclarationStatement(
                                                                    VariableDeclaration(
                                                                        GenericName(
                                                                            Identifier("List").WithLeadingTrivia(GenerateTabs(3)),
                                                                            TypeArgumentList(
                                                                                Token(LessThanToken),
                                                                                SeparatedList(new List<TypeSyntax>() { IdentifierName("ValidationResult") }),
                                                                                Token(GreaterThanToken).WithTrailingTrivia(Whitespace(" ")))),
                                                                        SeparatedList(
                                                                            new List<VariableDeclaratorSyntax>()
                                                                            {
                                                                                VariableDeclarator(Identifier("result").WithTrailingTrivia(Whitespace(" ")))
                                                                                    .WithInitializer(
                                                                                        EqualsValueClause(
                                                                                            Token(EqualsToken).WithTrailingTrivia(Whitespace(" ")),
                                                                                            InvocationExpression(
                                                                                                MemberAccessExpression(
                                                                                                    SimpleMemberAccessExpression,
                                                                                                    InvocationExpression(
                                                                                                        MemberAccessExpression(
                                                                                                            SimpleMemberAccessExpression,
                                                                                                            BaseExpression(Token(BaseKeyword)),
                                                                                                            Token(DotToken),
                                                                                                            IdentifierName("BUSUIValidate")),
                                                                                                        ArgumentList(
                                                                                                            Token(OpenParenToken),
                                                                                                            SeparatedList(
                                                                                                                new List<ArgumentSyntax>()
                                                                                                                {
                                                                                                                    Argument(IdentifierName("context")),
                                                                                                                    Argument(IdentifierName("businessEntity")).WithLeadingTrivia(Whitespace(" ")),
                                                                                                                    Argument(IdentifierName("UIEntity")).WithLeadingTrivia(Whitespace(" "))
                                                                                                                },
                                                                                                                GetSeparators(2, CommaToken)),
                                                                                                            Token(CloseParenToken))),
                                                                                                    Token(DotToken),
                                                                                                    IdentifierName("ToList")),
                                                                                                ArgumentList(
                                                                                                    Token(OpenParenToken),
                                                                                                    SeparatedList(new List<ArgumentSyntax>(){ }),
                                                                                                    Token(CloseParenToken)))))
                                                                            })))
                                                                .WithTrailingTrivia(LineFeed)
                                                            }

                                                            // Вовзращаемое значение
                                                            .Concat(new SyntaxNode[]
                                                            {
                                                                ReturnStatement(
                                                                    Token(ReturnKeyword)
                                                                        .WithLeadingTrivia(GenerateTabs(3))
                                                                        .WithTrailingTrivia(Whitespace(" ")),
                                                                    IdentifierName("result"),
                                                                    Token(SemicolonToken))
                                                                .WithTrailingTrivia(LineFeed)
                                                            })))

                                                        // Открывающая фигурная скобка метода
                                                        .WithOpenBraceToken(Token(
                                                            TriviaList(
                                                                GenerateTabs(2)),
                                                                OpenBraceToken,
                                                                TriviaList(LineFeed)))

                                                        // Закрывающая фигурная скобка метода
                                                        .WithCloseBraceToken(Token(
                                                            TriviaList(
                                                                GenerateTabs(2)),
                                                                CloseBraceToken,
                                                                TriviaList()))
                                                            .WithTrailingTrivia(LineFeed))
                                                #endregion
                                            }
                                        ))
                                        .WithOpenBraceToken(Token(OpenBraceToken)
                                            .WithLeadingTrivia(Tab)
                                            .WithTrailingTrivia(LineFeed))
                                        .WithCloseBraceToken(Token(CloseBraceToken)
                                            .WithLeadingTrivia(Tab)
                                            .WithTrailingTrivia(LineFeed))
                                }))
                            .WithOpenBraceToken(Token(OpenBraceToken).WithTrailingTrivia(LineFeed))
                            .WithCloseBraceToken(Token(CloseBraceToken).WithTrailingTrivia(LineFeed))))
                .WithFilePath(factoryPath);

            WriteTree(GSCrmApplication, factoryName, factoryType, syntaxTree);
            Compile("GSCrm" + factoryType, syntaxTree);                   
        }

        private static IEnumerable<SyntaxNode> MapRealColumnsToFields(List<Field> fields)
        {
            foreach (Field field in fields.Where(tc => tc.TableColumnId != null))
            {
                if (!IsSystemProperty(field.Name))
                {
                    yield return ExpressionStatement(
                        AssignmentExpression(
                            SimpleAssignmentExpression,
                            MemberAccessExpression(
                                SimpleMemberAccessExpression,
                                IdentifierName("businessEntity"),
                                Token(DotToken),
                                IdentifierName(field.Name)),
                            Token(EqualsToken)
                                .WithLeadingTrivia(Whitespace(" "))
                                .WithTrailingTrivia(Whitespace(" ")),
                            MemberAccessExpression(
                                SimpleMemberAccessExpression,
                                IdentifierName("dataEntity"),
                                Token(DotToken),
                                IdentifierName(field.TableColumn.Name))))
                    .WithLeadingTrivia(GenerateTabs(3))
                    .WithTrailingTrivia(LineFeed);
                }
            }
        }

        private static IEnumerable<StatementSyntax> InitializeCalcFields(List<Field> fields)
        {
            foreach (Field field in fields.Where(c => c.IsCalculate).ToList())
            {
                string fieldCalc = field.CalculatedValue.ToString();
                string resultCalcValue = string.Empty;
                foreach (string element in fieldCalc.Split("[&"))
                {
                    if (element.Contains("]"))
                    {
                        string[] elementParts = element.Split("]");
                        string property = string.Empty;
                        string endOfElement = string.Empty;
                        foreach (char c in element.TakeWhile(s => s.ToString() != "]"))
                            property += c;
                        foreach (char c in element.SkipWhile(s => s.ToString() != "]").Skip(1))
                            endOfElement += c;
                        if (fields.Select(n => n.Name).Contains(property))
                            resultCalcValue += $"businessEntity.{property}{endOfElement}";
                        
                        else resultCalcValue += property + endOfElement;
                    }
                    else resultCalcValue += element;
                }
                yield return ParseStatement($"{GenerateTabs(3)}businessEntity.{field.Name} = {resultCalcValue};\n");
            }
        }

        private static IEnumerable<SyntaxNode> MapJoinColumnsToFields(MainContext context, BusinessComponent businessComponent, List<Field> componentFields)
        {
            List<SyntaxNode> nodes = new List<SyntaxNode>();

            // Список полей компоненты, сгрупированных по join-у
            List<Field> initializedFields = new List<Field>();
            List<IGrouping<Guid?, Field>> groupsFields = componentFields.Where(jc => jc.JoinColumnId != null).GroupBy(j => j.JoinId).ToList();
            List<IGrouping<Guid?, Field>> sortedGroupsFields = new List<IGrouping<Guid?, Field>>();

            // Получение Join-ов в правильно последовательности необходимо при инициализации Join-а убедиться, что поле из спецификации уже было инициализировано до этого
            while(groupsFields.Count != sortedGroupsFields.Count)
            {
                groupsFields.Except(sortedGroupsFields).ToList().ForEach(fields =>
                {
                    Join join = fields.FirstOrDefault().Join;
                    JoinSpecification specification = join.JoinSpecifications.FirstOrDefault();
                    Field sorceField = componentFields.FirstOrDefault(i => i.Id == specification.SourceFieldId);

                    // Добавление поля в инициализированные
                    foreach (Field field in fields)
                        initializedFields.Add(field);

                    // Если поле основано на реальной колонке или находится в списке инициализированных, то все поля этого Join-а добавляются в список отсортированных
                    if (sorceField.TableColumnId != null || initializedFields.Contains(sorceField))
                        sortedGroupsFields.Add(fields);
                });
            }

            foreach (var groupFields in sortedGroupsFields)
            {
                Join join = groupFields.FirstOrDefault().Join;
                Table joinTable = join.Table;
                JoinSpecification joinSpecification = join.JoinSpecifications.FirstOrDefault();
                TableColumn destinationColumn = joinTable.TableColumns.FirstOrDefault(i => i.Id == joinSpecification.DestinationColumnId);
                Field sourceField = businessComponent.Fields.FirstOrDefault(i => i.Id == joinSpecification.SourceFieldId);
                string joinTableName = joinTable.Name;

                // Формирование списка полей
                List<StatementSyntax> joinedFieldsMapping = new List<StatementSyntax>();
                foreach (var field in groupFields)
                {
                    joinedFieldsMapping.Add(ExpressionStatement(
                        AssignmentExpression(
                            SimpleAssignmentExpression,
                            MemberAccessExpression(
                                SimpleMemberAccessExpression,
                                IdentifierName("businessEntity"),
                                Token(DotToken),
                                IdentifierName(field.Name).WithTrailingTrivia(Whitespace(" "))),
                            Token(EqualsToken).WithTrailingTrivia(Whitespace(" ")),
                            MemberAccessExpression(
                                SimpleMemberAccessExpression,
                                IdentifierName(join.Name),
                                Token(DotToken),
                                IdentifierName(field.JoinColumn.Name))))
                        .WithLeadingTrivia(Whitespace("\n                ")));
                }

                // Добавление узла получения join-а и заполнение join-ных колонок
                nodes.AddRange(new SyntaxNode[]
                {
                    LocalDeclarationStatement(
                        VariableDeclaration(
                            IdentifierName(joinTableName)
                                .WithLeadingTrivia(GenerateTabs(3))
                                .WithTrailingTrivia(Whitespace(" ")),
                            SeparatedList(
                                new List<VariableDeclaratorSyntax>()
                                {
                                    VariableDeclarator(Identifier(join.Name).WithTrailingTrivia(Whitespace(" ")))
                                        .WithInitializer(
                                            EqualsValueClause(
                                                Token(EqualsToken).WithTrailingTrivia(Whitespace(" ")),

                                                // Join к таблице
                                                InvocationExpression(
                                                    MemberAccessExpression(
                                                        SimpleMemberAccessExpression,
                                                        InvocationExpression(
                                                            MemberAccessExpression(
                                                                SimpleMemberAccessExpression,
                                                                MemberAccessExpression(
                                                                    SimpleMemberAccessExpression,
                                                                    IdentifierName("context"),
                                                                    Token(DotToken),
                                                                    IdentifierName(joinTableName)),
                                                                Token(DotToken),
                                                                IdentifierName("AsNoTracking")),
                                                            ArgumentList(
                                                                Token(OpenParenToken),
                                                                SeparatedList(
                                                                    new List<ArgumentSyntax>(){ }),
                                                                Token(CloseParenToken))),
                                                        Token(DotToken),
                                                        IdentifierName("FirstOrDefault")),
                                                    ArgumentList(
                                                        Token(OpenParenToken),
                                                        SeparatedList(
                                                            new List<ArgumentSyntax>()
                                                            {
                                                                Argument(
                                                                    SimpleLambdaExpression(Parameter(Identifier("i").WithTrailingTrivia(Whitespace(" "))))
                                                                                                        
                                                                        // Стрелочная функция
                                                                        .WithArrowToken(Token(EqualsGreaterThanToken))

                                                                        // Тело выражения
                                                                        .WithExpressionBody(
                                                                            BinaryExpression(
                                                                                EqualsExpression,
                                                                                MemberAccessExpression(
                                                                                    SimpleMemberAccessExpression,
                                                                                    IdentifierName("i").WithLeadingTrivia(Whitespace(" ")),
                                                                                    Token(DotToken),
                                                                                    IdentifierName(destinationColumn.Name)),
                                                                                ParseToken("==")
                                                                                    .WithLeadingTrivia(Whitespace(" "))
                                                                                    .WithTrailingTrivia(Whitespace(" ")),
                                                                                MemberAccessExpression(
                                                                                    SimpleMemberAccessExpression,
                                                                                    IdentifierName("businessEntity"),
                                                                                    Token(DotToken),
                                                                                    IdentifierName(sourceField.Name)))))
                                                            }),
                                                        Token(CloseParenToken)))))
                                }))).WithTrailingTrivia(LineFeed),
                    IfStatement(
                        Token(IfKeyword)
                            .WithLeadingTrivia(GenerateTabs(3))
                            .WithTrailingTrivia(Whitespace(" ")),
                        Token(OpenParenToken),
                        BinaryExpression(
                            NotEqualsExpression,
                            IdentifierName(join.Name).WithTrailingTrivia(Whitespace(" ")),
                            Token(ExclamationEqualsToken).WithTrailingTrivia(Whitespace(" ")),
                            LiteralExpression(
                                NullLiteralExpression,
                                Token(NullKeyword))),
                        Token(CloseParenToken).WithTrailingTrivia(LineFeed),
                        Block(
                            Token(OpenBraceToken).WithLeadingTrivia(GenerateTabs(3)),
                            List(joinedFieldsMapping),
                            Token(CloseBraceToken).WithLeadingTrivia(Whitespace("\n            "))),
                        default)
                    .WithLeadingTrivia(GenerateTabs(3))
                    .WithTrailingTrivia(LineFeed)
                });
            }
            return nodes;
        }

        private static IEnumerable<SyntaxNode> MapFieldsToTableColumns(List<Field> fields)
        {
            foreach (Field field in fields)
            {
                if (!IsSystemProperty(field.Name) && field.TableColumn != null)
                {
                    yield return ExpressionStatement(
                        AssignmentExpression(
                            SimpleAssignmentExpression,
                            MemberAccessExpression(
                                SimpleMemberAccessExpression,
                                IdentifierName("dataEntity"),
                                Token(DotToken),
                                IdentifierName(field.TableColumn.Name)),
                            Token(EqualsToken)
                                .WithLeadingTrivia(Whitespace(" "))
                                .WithTrailingTrivia(Whitespace(" ")),
                            MemberAccessExpression(
                                SimpleMemberAccessExpression,
                                IdentifierName("businessEntity"),
                                Token(DotToken),
                                IdentifierName(field.Name))))
                    .WithLeadingTrivia(GenerateTabs(3))
                    .WithTrailingTrivia(LineFeed);
                }
            }
        }
        
        private static IEnumerable<SyntaxNode> MapControlsToFields(List<Control> controls, List<Column> columns)
        {
            foreach(Control control in controls.Where(t => t.Type != "button").Where(f => f.Field != null))
            {
                if (!IsSystemProperty(control.Name))
                {
                    yield return ExpressionStatement(
                        AssignmentExpression(
                            SimpleAssignmentExpression,
                            MemberAccessExpression(
                                SimpleMemberAccessExpression,
                                IdentifierName("businessEntity"),
                                Token(DotToken),
                                IdentifierName(control.Field.Name)),
                            Token(EqualsToken)
                                .WithLeadingTrivia(Whitespace(" "))
                                .WithTrailingTrivia(Whitespace(" ")),
                            MemberAccessExpression(
                                SimpleMemberAccessExpression,
                                IdentifierName("UIEntity"),
                                Token(DotToken),
                                IdentifierName(control.Name))))
                    .WithLeadingTrivia(GenerateTabs(3))
                    .WithTrailingTrivia(LineFeed);
                }
            }
            
            foreach(Column column in columns.Where(t => t.Type != "button").Where(f => f.Field != null))
            {
                if (!IsSystemProperty(column.Name))
                {
                    yield return ExpressionStatement(
                        AssignmentExpression(
                            SimpleAssignmentExpression,
                            MemberAccessExpression(
                                SimpleMemberAccessExpression,
                                IdentifierName("businessEntity"),
                                Token(DotToken),
                                IdentifierName(column.Field.Name)),
                            Token(EqualsToken)
                                .WithLeadingTrivia(Whitespace(" "))
                                .WithTrailingTrivia(Whitespace(" ")),
                            MemberAccessExpression(
                                SimpleMemberAccessExpression,
                                IdentifierName("UIEntity"),
                                Token(DotToken),
                                IdentifierName(column.Name))))
                    .WithLeadingTrivia(GenerateTabs(3))
                    .WithTrailingTrivia(LineFeed);
                }
            }
        }

        private static IEnumerable<SyntaxNode> MapFieldsToControls(List<Control> controls, List<Column> columns)
        {
            foreach (Control control in controls.Where(t => t.Type != "button").Where(f => f.Field != null))
            {
                if (!IsSystemProperty(control.Name))
                {
                    yield return ExpressionStatement(
                        AssignmentExpression(
                            SimpleAssignmentExpression,
                            MemberAccessExpression(
                                SimpleMemberAccessExpression,
                                IdentifierName("UIEntity"),
                                Token(DotToken),
                                IdentifierName(control.Name)),
                            Token(EqualsToken)
                                .WithLeadingTrivia(Whitespace(" "))
                                .WithTrailingTrivia(Whitespace(" ")),
                            MemberAccessExpression(
                                SimpleMemberAccessExpression,
                                IdentifierName("businessEntity"),
                                Token(DotToken),
                                IdentifierName(control.Field.Name))))
                    .WithLeadingTrivia(GenerateTabs(3))
                    .WithTrailingTrivia(LineFeed);
                }
            }

            foreach (Column column in columns.Where(t => t.Type != "button").Where(f => f.Field != null))
            {
                if (!IsSystemProperty(column.Name))
                {
                    yield return ExpressionStatement(
                        AssignmentExpression(
                            SimpleAssignmentExpression,
                            MemberAccessExpression(
                                SimpleMemberAccessExpression,
                                IdentifierName("UIEntity"),
                                Token(DotToken),
                                IdentifierName(column.Name)),
                            Token(EqualsToken)
                                .WithLeadingTrivia(Whitespace(" "))
                                .WithTrailingTrivia(Whitespace(" ")),
                            MemberAccessExpression(
                                SimpleMemberAccessExpression,
                                IdentifierName("businessEntity"),
                                Token(DotToken),
                                IdentifierName(column.Field.Name))))
                    .WithLeadingTrivia(GenerateTabs(3))
                    .WithTrailingTrivia(LineFeed);
                }
            }
        }

        private static IEnumerable<SyntaxNode> GetReadonlyChecks(BusinessComponent businessEntity)
        {
            foreach(Field field in businessEntity.Fields.Where(f => f.Readonly && f.TableColumn != null).ToList())
            {
                yield return IfStatement(
                    Token(IfKeyword)
                        .WithLeadingTrivia(GenerateTabs(3))
                        .WithTrailingTrivia(Whitespace(" ")),
                    Token(OpenParenToken),
                    BinaryExpression(
                        NotEqualsExpression,
                        MemberAccessExpression(
                            SimpleMemberAccessExpression,
                            IdentifierName("dataEntity"),
                            Token(DotToken),
                            IdentifierName(field.TableColumn.Name))
                                .WithTrailingTrivia(Whitespace(" ")),
                        ParseToken("!=").WithTrailingTrivia(Whitespace(" ")),
                        MemberAccessExpression(
                            SimpleMemberAccessExpression,
                            IdentifierName("businessEntity"),
                            Token(DotToken),
                            IdentifierName(field.Name))),
                    Token(CloseParenToken).WithTrailingTrivia(LineFeed),
                    ExpressionStatement(
                        InvocationExpression(
                            MemberAccessExpression(
                                SimpleMemberAccessExpression,
                                IdentifierName("result").WithLeadingTrivia(GenerateTabs(4)),
                                Token(DotToken),
                                IdentifierName("Add")),
                            ArgumentList(
                                Token(OpenParenToken),
                                SeparatedList(
                                    new List<ArgumentSyntax>()
                                    {
                                        Argument(
                                            ObjectCreationExpression(
                                                Token(NewKeyword).WithTrailingTrivia(Whitespace(" ")),
                                                ParseTypeName("ValidationResult"),
                                                ArgumentList(
                                                    Token(OpenParenToken).WithTrailingTrivia(LineFeed),
                                                    SeparatedList(
                                                        new List<ArgumentSyntax>()
                                                        {
                                                            Argument(LiteralExpression(StringLiteralExpression, ParseToken($"\"Field {field.Name} is a readonly field.\"")))
                                                                    .WithLeadingTrivia(GenerateTabs(5)),

                                                            Argument(
                                                                ObjectCreationExpression(
                                                                    Token(NewKeyword)
                                                                        .WithLeadingTrivia(GenerateTabs(5))
                                                                        .WithTrailingTrivia(Whitespace(" ")),
                                                                    GenericName(
                                                                        Identifier("List"),
                                                                        TypeArgumentList(
                                                                            Token(LessThanToken),
                                                                            SeparatedList(new List<TypeSyntax>() { ParseTypeName("string") }),
                                                                            Token(GreaterThanToken))),
                                                                    ArgumentList(
                                                                        Token(OpenParenToken),
                                                                        SeparatedList(new List<ArgumentSyntax>(){ }),
                                                                        Token(CloseParenToken).WithTrailingTrivia(Whitespace(" "))),
                                                                    InitializerExpression(
                                                                        CollectionInitializerExpression,
                                                                        Token(OpenBraceToken).WithTrailingTrivia(Whitespace(" ")),
                                                                        SeparatedList(
                                                                            new List<ExpressionSyntax>()
                                                                            {
                                                                                LiteralExpression(StringLiteralExpression, ParseToken($"\"{field.Name}\""))
                                                                                    .WithTrailingTrivia(Whitespace(" "))
                                                                            }),
                                                                        Token(CloseBraceToken))))
                                                        },
                                                        GetSeparators(1, CommaToken, LineFeed)),
                                                    Token(CloseParenToken)),
                                                default))
                                    }),
                                Token(CloseParenToken))))
                        .WithTrailingTrivia(LineFeed),
                    default);
            }
        }
    }
}