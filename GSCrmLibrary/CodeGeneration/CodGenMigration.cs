using System.Linq;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxKind;
using static GSCrmLibrary.CodeGeneration.CodGenTreesUtils;
using GSCrmLibrary.Models.TableModels;
using static GSCrmLibrary.Configuration.ApplicationConfig;
using GSCrmLibrary.Data;
using Microsoft.EntityFrameworkCore;

namespace GSCrmLibrary.CodeGeneration
{
    public static class CodGenMigration
    {
        public static void CreateMigration(MainContext context, Table table, string fileName)
        {
            IEnumerable<AnonymousObjectMemberDeclaratorSyntax> columns = GetColumns(table);

            CompilationUnitSyntax unit = CompilationUnit()
                .AddUsings(
                    UsingDirective(
                        ParseName("System").WithLeadingTrivia(Whitespace(" "))
                    ).WithTrailingTrivia(LineFeed),
                    UsingDirective(
                        ParseName("Microsoft.EntityFrameworkCore").WithLeadingTrivia(Whitespace(" "))
                    ).WithTrailingTrivia(LineFeed),
                    UsingDirective(
                        ParseName("Microsoft.EntityFrameworkCore.Infrastructure").WithLeadingTrivia(Whitespace(" "))
                    ).WithTrailingTrivia(LineFeed),
                    UsingDirective(
                        ParseName("Microsoft.EntityFrameworkCore.Migrations").WithLeadingTrivia(Whitespace(" "))
                    ).WithTrailingTrivia(LineFeed),
                    UsingDirective(
                        ParseName("GSCrmApplication.Data").WithLeadingTrivia(Whitespace(" "))
                    ).WithTrailingTrivia(Whitespace("\n\n")))
                .AddMembers(
                    NamespaceDeclaration(
                        QualifiedName(
                            IdentifierName("GSCrm"),
                            Token(DotToken),
                            IdentifierName("GenerateLibraries")))
                        .WithTrailingTrivia(LineFeed)

                        // Открывающая фигурная скобка
                        .WithOpenBraceToken(Token(OpenBraceToken).WithTrailingTrivia(LineFeed))

                        // Ключевое слово
                        .WithNamespaceKeyword(Token(NamespaceKeyword).WithTrailingTrivia(Whitespace(" ")))

                        // Классы, входящие в namespace
                        .WithMembers(List(
                            new List<MemberDeclarationSyntax>()
                            {
                                // Класс миграции
                                ClassDeclaration(ParseToken(fileName).WithTrailingTrivia(Whitespace(" ")))
                                    // Открывающая фигурная скобка класса
                                    .WithOpenBraceToken(Token(OpenBraceToken)
                                        .WithLeadingTrivia(Tab)
                                        .WithTrailingTrivia(LineFeed))

                                    // Модификатор доступа
                                    .WithModifiers(
                                        TokenList(
                                            Token(PublicKeyword)
                                                .WithLeadingTrivia(Tab)
                                                .WithTrailingTrivia(Whitespace(" ")),
                                            Token(PartialKeyword).WithTrailingTrivia(Whitespace(" "))))

                                    // Ключевое слово
                                    .WithKeyword(Token(ClassKeyword).WithTrailingTrivia(Whitespace(" ")))

                                    // Базовый класс
                                    .WithBaseList(
                                        BaseList(
                                            SingletonSeparatedList<BaseTypeSyntax>(
                                                SimpleBaseType(
                                                    IdentifierName(
                                                        Identifier(
                                                            TriviaList(Whitespace(" ")),
                                                            "Migration",
                                                            TriviaList(LineFeed)))))))

                                    // Атрибуты
                                    .WithAttributeLists(List(
                                        new List<AttributeListSyntax>()
                                        {
                                            // Атрибут с типом контекста
                                            AttributeList(
                                                Token(OpenBracketToken).WithLeadingTrivia(Tab),
                                                default,
                                                SeparatedList(new List<AttributeSyntax>()
                                                {
                                                    Attribute(
                                                        IdentifierName("DbContext"),
                                                        AttributeArgumentList(
                                                            Token(OpenParenToken),
                                                            SeparatedList(
                                                                new List<AttributeArgumentSyntax>()
                                                                {
                                                                    AttributeArgument(
                                                                        TypeOfExpression(
                                                                            Token(TypeOfKeyword),
                                                                            Token(OpenParenToken),
                                                                            ParseTypeName("GSAppContext"),
                                                                            Token(CloseParenToken))),
                                                                }),
                                                            Token(CloseParenToken))),
                                                }),
                                                Token(CloseBracketToken)).WithTrailingTrivia(LineFeed),

                                            // Атрибут с названием миграции
                                            AttributeList(
                                                Token(OpenBracketToken).WithLeadingTrivia(Tab),
                                                default,
                                                SeparatedList(new List<AttributeSyntax>()
                                                {
                                                    Attribute(
                                                        IdentifierName("Migration"),
                                                        AttributeArgumentList(
                                                            Token(OpenParenToken),
                                                            SeparatedList(
                                                                new List<AttributeArgumentSyntax>()
                                                                {
                                                                    AttributeArgument(
                                                                        LiteralExpression(
                                                                            StringLiteralExpression,
                                                                            Literal(fileName))),
                                                                }),
                                                            Token(CloseParenToken))),
                                                }),
                                                Token(CloseBracketToken)).WithTrailingTrivia(LineFeed),
                                        }))

                                    // Методы класса
                                    .WithMembers(List(
                                        new List<MemberDeclarationSyntax>()
                                        {
                                            {
                                                #region Поле для хранения контекста приложения
                                                FieldDeclaration(
                                                    VariableDeclaration(
                                                        ParseTypeName("GSAppContext").WithTrailingTrivia(Whitespace(" ")),
                                                        SeparatedList(
                                                            new List<VariableDeclaratorSyntax>()
                                                            {
                                                                VariableDeclarator(ParseToken("context"))                                                          
                                                            },
                                                            new List<SyntaxToken>(){ } )))

                                                    // Модификатор доступа и слово readonly
                                                    .WithModifiers(
                                                        TokenList(
                                                            Token(PrivateKeyword)
                                                                .WithLeadingTrivia(GenerateTabs(2))
                                                                .WithTrailingTrivia(Whitespace(" ")),
                                                            Token(ReadOnlyKeyword).WithTrailingTrivia(Whitespace(" "))))
                                                    .WithSemicolonToken(Token(SemicolonToken).WithTrailingTrivia(LineFeed))
                                                #endregion                                            
                                            },
                                            {
                                                #region Констркутор класса без параметров
                                                ConstructorDeclaration(fileName)

                                                    // Модификатор доступа
                                                    .WithModifiers(
                                                        TokenList(
                                                            Token(PublicKeyword)
                                                                .WithLeadingTrivia(Whitespace("\n        "))
                                                                .WithTrailingTrivia(Whitespace(" "))))

                                                    // Список параметров конструктора
                                                    .WithParameterList(
                                                        ParameterList()
                                                            .WithOpenParenToken(Token(OpenParenToken))
                                                            .WithCloseParenToken(Token(CloseParenToken)))


                                                    .WithBody(
                                                        Block(new List<StatementSyntax>(){ })
                                                    
                                                        // Открывающая фигурная скобка конструктора
                                                        .WithOpenBraceToken(Token(
                                                            TriviaList(Whitespace(" ")),
                                                            OpenBraceToken,
                                                            TriviaList(Whitespace(" "))))

                                                        // Закрывающая фигурная скобка конструктора
                                                        .WithCloseBraceToken(Token(CloseBraceToken)))
                                                #endregion
                                            },
                                            {
                                                #region Конструктор класса, принимающий контекст приложения
                                                ConstructorDeclaration(fileName)

                                                    // Модификатор доступа
                                                    .WithModifiers(
                                                        TokenList(
                                                            Token(PublicKeyword)
                                                                .WithLeadingTrivia(Whitespace("\n\n        "))
                                                                .WithTrailingTrivia(Whitespace(" "))))

                                                    // Список параметров конструктора
                                                    .WithParameterList(
                                                        ParameterList()
                                                            .WithOpenParenToken(Token(OpenParenToken))
                                                            .WithParameters(
                                                                SingletonSeparatedList(
                                                                    Parameter(
                                                                        List(new List<AttributeListSyntax>(){ }),
                                                                        TokenList(),
                                                                        ParseTypeName("GSAppContext").WithTrailingTrivia(Whitespace(" ")),
                                                                        Identifier("context"),
                                                                        default)))
                                                            .WithCloseParenToken(Token(CloseParenToken)).WithTrailingTrivia(LineFeed))

                                                    // Тело конструктора
                                                    .WithBody(
                                                        Block(List(
                                                            new List<StatementSyntax>()
                                                            {
                                                                ExpressionStatement(
                                                                    AssignmentExpression(
                                                                        SimpleAssignmentExpression,
                                                                        MemberAccessExpression(
                                                                            SimpleMemberAccessExpression,
                                                                            ThisExpression(Token(ThisKeyword).WithLeadingTrivia(GenerateTabs(3))),
                                                                            Token(DotToken),
                                                                            IdentifierName("context").WithTrailingTrivia(Whitespace(" "))),
                                                                        Token(EqualsToken).WithTrailingTrivia(Whitespace(" ")),
                                                                        IdentifierName("context")),
                                                                    Token(SemicolonToken).WithTrailingTrivia(LineFeed))
                                                            }))

                                                        // Открывающая фигурная скобка конструктора
                                                        .WithOpenBraceToken(Token(
                                                            TriviaList(GenerateTabs(2)),
                                                            OpenBraceToken,
                                                            TriviaList(LineFeed)))

                                                        // Закрывающая фигурная скобка конструктора
                                                        .WithCloseBraceToken(Token(
                                                            TriviaList(GenerateTabs(2)),
                                                            CloseBraceToken,
                                                            TriviaList())))
                                                #endregion
                                            },
                                            {
                                                #region Объявление метода миграции бд
                                                MethodDeclaration(
                                                    PredefinedType(
                                                        Token(
                                                            TriviaList(),
                                                            VoidKeyword,
                                                            TriviaList(Whitespace(" ")))), 
                                                        "Up")

                                                    // Модификатор доступа и ключевое слово override
                                                    .WithModifiers(
                                                        TokenList(
                                                            Token(ProtectedKeyword)
                                                                .WithTrailingTrivia(Whitespace(" "))
                                                                .WithLeadingTrivia(Whitespace("\n\n        ")),
                                                            Token(OverrideKeyword).WithTrailingTrivia(Whitespace(" "))))

                                                    // Список параметров
                                                    .WithParameterList(
                                                        ParameterList()
                                                            .WithOpenParenToken(Token(OpenParenToken))
                                                            .WithParameters(
                                                                SingletonSeparatedList(
                                                                    Parameter(
                                                                        List(new List<AttributeListSyntax>(){ }),
                                                                        TokenList(),
                                                                        ParseTypeName("MigrationBuilder").WithTrailingTrivia(Whitespace(" ")),
                                                                        Identifier("migrationBuilder"),
                                                                        default)))
                                                            .WithCloseParenToken(Token(CloseParenToken)).WithTrailingTrivia(LineFeed))

                                                    // Тело метода
                                                    .WithBody(
                                                        Block(List(
                                                            !table.IsApply ?
                                                            #region Утверждение - вызов метода CreateTable 
                                                            new List<StatementSyntax>()
                                                            {
                                                                ExpressionStatement(
                                                                    InvocationExpression(
                                                                        MemberAccessExpression(
                                                                            SimpleMemberAccessExpression,
                                                                            IdentifierName("migrationBuilder"),
                                                                            Token(DotToken),
                                                                            IdentifierName("CreateTable")),

                                                                        // Аргументы, передаваемые в вызываемый метод
                                                                        ArgumentList(
                                                                            SeparatedList(
                                                                                // Аргументы
                                                                                new List<ArgumentSyntax>()
                                                                                {
                                                                                    // Название таблицы
                                                                                    #region TableName
                                                                                    Argument(
                                                                                        
                                                                                        // Название переменной, для которой передается значение
                                                                                        NameColon(
                                                                                            IdentifierName(ParseToken("name").WithLeadingTrivia(Whitespace("\n                "))),
                                                                                            Token(ColonToken)),
                                                                                        default,
                                                                                        
                                                                                        // Лямбда выражение
                                                                                        LiteralExpression(
                                                                                            StringLiteralExpression, 
                                                                                            Literal(table.Name).WithLeadingTrivia(Whitespace(" ")))),
                                                                                    #endregion

                                                                                    // Колонки таблицы
                                                                                    #region TableColumns
                                                                                    Argument(

                                                                                        // Название переменной, для которой передается значение
                                                                                        NameColon(
                                                                                            IdentifierName(ParseToken("columns").WithLeadingTrivia(Whitespace("\n                "))),
                                                                                            Token(ColonToken)),
                                                                                        default,
                                                                                        
                                                                                        // Лямбда выражение
                                                                                        SimpleLambdaExpression(Parameter(Identifier("table").WithLeadingTrivia(Whitespace(" "))))

                                                                                            // Стрелочная функция
                                                                                            .WithArrowToken(Token(EqualsGreaterThanToken)
                                                                                                .WithLeadingTrivia(Whitespace(" "))
                                                                                                .WithTrailingTrivia(Whitespace(" ")))
                                                                                            
                                                                                            // Тело выражения
                                                                                            .WithExpressionBody(
                                                                                                AnonymousObjectCreationExpression()
                                                                                                    .WithInitializers(SeparatedList(columns, GetSeparators(columns.Count() - 1, CommaToken)))
                                                                                                    .WithOpenBraceToken(Token(OpenBraceToken).WithLeadingTrivia(Whitespace("\n                ")))
                                                                                                    .WithCloseBraceToken(Token(CloseBraceToken).WithLeadingTrivia(Whitespace("\n                "))))),
                                                                                    #endregion

                                                                                    // Ограничения
                                                                                    #region TableConstraints
                                                                                    Argument(
                                                                                        
                                                                                        // Название переменной, для которой передается значение
                                                                                        NameColon(
                                                                                            IdentifierName(ParseToken("constraints").WithLeadingTrivia(Whitespace("\n                "))),
                                                                                            Token(ColonToken)),
                                                                                        default,
                                                                                        
                                                                                        // Лямбда выражение
                                                                                        SimpleLambdaExpression(Parameter(Identifier("table").WithLeadingTrivia(Whitespace(" "))))

                                                                                            // Стрелочная функция
                                                                                            .WithArrowToken(Token(EqualsGreaterThanToken)
                                                                                                .WithLeadingTrivia(Whitespace(" "))
                                                                                                .WithTrailingTrivia(LineFeed))

                                                                                            // Тело выражения
                                                                                            .WithBlock(
                                                                                                Block(List(
                                                                                                    new List<StatementSyntax>()
                                                                                                    {
                                                                                                        ExpressionStatement(
                                                                                                            InvocationExpression(
                                                                                                                MemberAccessExpression(
                                                                                                                    SimpleMemberAccessExpression,
                                                                                                                    IdentifierName("table").WithLeadingTrivia(GenerateTabs(5)),
                                                                                                                    Token(DotToken),
                                                                                                                    IdentifierName("PrimaryKey")),
                                                                                                                ArgumentList(
                                                                                                                    SeparatedList(
                                                                                                                        new List<ArgumentSyntax>()
                                                                                                                        {
                                                                                                                            Argument(LiteralExpression(StringLiteralExpression, ParseToken("\"PK_" + table.Name + "\""))),
                                                                                                                            Argument(
                                                                                                                                SimpleLambdaExpression(
                                                                                                                                    Parameter(Identifier("x")
                                                                                                                                        .WithLeadingTrivia(Whitespace(" "))
                                                                                                                                        .WithTrailingTrivia(Whitespace(" "))))
                                                                                                                                .WithArrowToken(Token(EqualsGreaterThanToken).WithTrailingTrivia(Whitespace(" ")))
                                                                                                                                .WithExpressionBody(
                                                                                                                                    MemberAccessExpression(
                                                                                                                                        SimpleMemberAccessExpression,
                                                                                                                                        IdentifierName("x"),
                                                                                                                                        Token(DotToken),
                                                                                                                                        IdentifierName("Id"))))
                                                                                                                        },
                                                                                                                        GetSeparators(1, CommaToken)))
                                                                                                                .WithOpenParenToken(Token(OpenParenToken))
                                                                                                                .WithCloseParenToken(Token(CloseParenToken))),
                                                                                                            Token(SemicolonToken).WithTrailingTrivia(LineFeed))
                                                                                                    }
                                                                                                    
                                                                                                    // Ключи
                                                                                                    .Concat(GetKeys(context, table))))
                                                                                                
                                                                                                // Открывающая/закрывающая фигурная скобка для тела выражения
                                                                                                .WithOpenBraceToken(Token(OpenBraceToken)
                                                                                                    .WithLeadingTrivia(GenerateTabs(4))
                                                                                                    .WithTrailingTrivia(LineFeed))
                                                                                                .WithCloseBraceToken(Token(CloseBraceToken).WithLeadingTrivia(GenerateTabs(4))))),
                                                                                    #endregion
                                                                                },

                                                                                // Список разделителей
                                                                                GetSeparators(2, CommaToken)))
                                                                        
                                                                        // Открывающая/закрывающая скобка для вызываемого метода, куда передается список аргументов
                                                                        .WithOpenParenToken(Token(OpenParenToken))
                                                                        .WithCloseParenToken(Token(CloseParenToken))),
                                                                    Token(SemicolonToken))

                                                                // Отступы для утверждения
                                                                .WithLeadingTrivia(GenerateTabs(3))
                                                                .WithTrailingTrivia(LineFeed)
                                                            }
                                                            #endregion

                                                            :
                                                            #region Утверждение - вызов метода AlterColumn или AddColumn
                                                            AlterColumns(table)
                                                            #endregion                                                            
                                                            ))

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
                                                                TriviaList())))
                                                #endregion
                                            },
                                            /*{
                                                #region Объявление метода обновления бд
                                                MethodDeclaration(
                                                    PredefinedType(
                                                        Token(
                                                            TriviaList(),
                                                            VoidKeyword,
                                                            TriviaList(Space))), 
                                                    "Migrate")

                                                    // Модификатор доступа
                                                    .WithModifiers(
                                                        TokenList(
                                                            Token(PublicKeyword)
                                                                .WithLeadingTrivia(Whitespace("\n\n        "))
                                                                .WithTrailingTrivia(Whitespace(" "))))

                                                    // Список параметров
                                                    .WithParameterList(
                                                        ParameterList()
                                                            .WithOpenParenToken(Token(OpenParenToken))
                                                            .WithCloseParenToken(Token(CloseParenToken)).WithTrailingTrivia(LineFeed))

                                                    // Тело метода
                                                    .WithBody(
                                                        Block(List(
                                                            new List<StatementSyntax>()
                                                            {
                                                                // Миграция бд на новую схему
                                                                ExpressionStatement(
                                                                    InvocationExpression(
                                                                        MemberAccessExpression(
                                                                            SimpleMemberAccessExpression,
                                                                            MemberAccessExpression(
                                                                                SimpleMemberAccessExpression,
                                                                                IdentifierName("context").WithLeadingTrivia(GenerateTabs(3)),
                                                                                Token(DotToken),
                                                                                IdentifierName("Database")),
                                                                            Token(DotToken),
                                                                            IdentifierName("Migrate")),
                                                                        ArgumentList(
                                                                            Token(OpenParenToken),
                                                                            SeparatedList(new List<ArgumentSyntax>(){ }),
                                                                            Token(CloseParenToken))),
                                                                    Token(SemicolonToken).WithTrailingTrivia(LineFeed))
                                                            }))

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
                                                            TriviaList())))
                                                #endregion
                                            }*/
                                        }))
                                    
                                    // Закрывающая фигурная скобка класса
                                    .WithCloseBraceToken(Token(CloseBraceToken)
                                        .WithLeadingTrivia(ParseLeadingTrivia("\n    "))
                                        .WithTrailingTrivia(LineFeed))
                            }))

                        // Закрывающая фигурная скобка
                        .WithCloseBraceToken(Token(CloseBraceToken).WithTrailingTrivia(LineFeed)));

            CodGenUtils.WriteTree(GSCrmApplication, fileName, "Migration", unit.SyntaxTree);
            CodGenUtils.Compile("GSCrmTable", unit.SyntaxTree);
        }

        private static IEnumerable<AnonymousObjectMemberDeclaratorSyntax> GetColumns(Table table)
        {
            foreach (TableColumn column in table.TableColumns)
            {
                yield return AnonymousObjectMemberDeclarator(
                    // Название колонки и знак =
                    NameEquals(
                        IdentifierName(column.Name)
                            .WithLeadingTrivia(Whitespace("\n                    "))
                            .WithTrailingTrivia(Whitespace(" ")),
                        Token(EqualsToken).WithTrailingTrivia(Whitespace(" "))),

                    // Значение, присваиваемое колонке
                    InvocationExpression(
                        // Выозов метода Column
                        MemberAccessExpression(
                            SimpleMemberAccessExpression,
                            IdentifierName("table"),
                            Token(DotToken),
                            GenericName(
                                Identifier("Column"),
                                TypeArgumentList(
                                    Token(LessThanToken),
                                    SeparatedList(
                                        new List<TypeSyntax>()
                                        {
                                            ParseTypeName(column.Type == "String" ? "string" : column.Type)
                                        }),
                                    Token(GreaterThanToken)))),

                        // Аргументы
                        ArgumentList(
                            SeparatedList(
                                new List<ArgumentSyntax>()
                                {
                                    Argument(
                                        NameColon(
                                            IdentifierName("nullable"),
                                            Token(ColonToken).WithTrailingTrivia(Whitespace(" "))),
                                        default,

                                        column.IsNullable ?
                                        LiteralExpression(
                                            TrueLiteralExpression,
                                            Token(TrueKeyword))
                                        :
                                        LiteralExpression(
                                            FalseLiteralExpression,
                                            Token(FalseKeyword))),
                                    Argument(
                                        NameColon(
                                            IdentifierName("maxLength"),
                                            Token(ColonToken).WithTrailingTrivia(Whitespace(" "))),
                                        default,
                                        LiteralExpression(
                                            NumericLiteralExpression,
                                            ParseToken(column.Length.ToString())))
                                },
                                GetSeparators(1, CommaToken, Whitespace(" "))))
                        .WithOpenParenToken(Token(OpenParenToken))
                        .WithCloseParenToken(Token(CloseParenToken))));
            }
        }

        private static IEnumerable<SyntaxNode> AlterColumns(Table table)
        {
            foreach (TableColumn column in table.TableColumns.Where(flag => flag.NeedCreate == true || flag.NeedUpdate == true))
            {
                yield return
                    #region AddColumn
                    column.NeedCreate ?
                    ExpressionStatement(
                        InvocationExpression(
                            MemberAccessExpression(
                                SimpleMemberAccessExpression,
                                IdentifierName("migrationBuilder"),
                                Token(DotToken),
                                GenericName(
                                    ParseToken("AddColumn"),
                                    TypeArgumentList(
                                        Token(LessThanToken),
                                        SeparatedList(
                                            new List<TypeSyntax>()
                                            {
                                                ParseTypeName(column.Type)
                                            },
                                            new List<SyntaxToken>(){ }),
                                        Token(GreaterThanToken)))),

                            // Аргументы, передаваемые в вызываемый метод
                            ArgumentList(
                                SeparatedList(
                                    // Аргументы
                                    GetColumnProperties(table, column),

                                    // Список разделителей
                                    GetSeparators(3, CommaToken)))

                            // Открывающая/закрывающая скобка для вызываемого метода, куда передается список аргументов
                            .WithOpenParenToken(Token(OpenParenToken))
                            .WithCloseParenToken(Token(CloseParenToken))),
                        ParseToken(";"))

                    // Отступы для утверждения
                    .WithLeadingTrivia(GenerateTabs(3))
                    .WithTrailingTrivia(Whitespace("\n"))
                    #endregion
                    :
                    #region AlterColumn
                    ExpressionStatement(
                        InvocationExpression(
                            MemberAccessExpression(
                                SimpleMemberAccessExpression,
                                IdentifierName("migrationBuilder"),
                                Token(DotToken),
                                GenericName(
                                    ParseToken("AlterColumn"),
                                    TypeArgumentList(
                                        Token(LessThanToken),
                                        SeparatedList(
                                            new List<TypeSyntax>()
                                            {
                                                ParseTypeName(column.Type)
                                            },
                                            new List<SyntaxToken>(){ }),
                                        Token(GreaterThanToken)))),

                            // Аргументы, передаваемые в вызываемый метод
                            ArgumentList(
                                SeparatedList(
                                    // Аргументы
                                    GetColumnProperties(table, column),

                                    // Список разделителей
                                    GetSeparators(3, CommaToken)))

                            // Открывающая/закрывающая скобка для вызываемого метода, куда передается список аргументов
                            .WithOpenParenToken(Token(OpenParenToken))
                            .WithCloseParenToken(Token(CloseParenToken))),
                        ParseToken(";"))

                    // Отступы для утверждения
                    .WithLeadingTrivia(GenerateTabs(3))
                    .WithTrailingTrivia(Whitespace("\n"));
                    #endregion
            }
        }

        private static IEnumerable<SyntaxNode> GetColumnProperties(Table table, TableColumn column)
        {
            return new List<ArgumentSyntax>()
            {
                #region ColumnName
                Argument(
                                            
                    // Название переменной, для которой передается значение
                    NameColon(
                        IdentifierName(ParseToken("name").WithLeadingTrivia(Whitespace("\n                "))),
                        Token(ColonToken)),
                    default,
                                            
                // Лямбда выражение
                LiteralExpression(
                    StringLiteralExpression,
                    Literal(column.Name).WithLeadingTrivia(Whitespace(" ")))),
                #endregion

                #region TableName
                Argument(
                                            
                    // Название переменной, для которой передается значение
                    NameColon(
                        IdentifierName(ParseToken("table").WithLeadingTrivia(Whitespace("\n                "))),
                        Token(ColonToken)),
                    default,
                                            
                    // Лямбда выражение
                    LiteralExpression(
                        StringLiteralExpression,
                        Literal(table.Name).WithLeadingTrivia(Whitespace(" ")))),
                #endregion

                #region MaxLength
                Argument(
                                            
                    // Название переменной, для которой передается значение
                    NameColon(
                        IdentifierName(ParseToken("maxLength").WithLeadingTrivia(Whitespace("\n                "))),
                        Token(ColonToken)),
                    default,
                                            
                    // Лямбда выражение
                    LiteralExpression(
                        NumericLiteralExpression,
                        Literal(column.Length).WithLeadingTrivia(Whitespace(" ")))),
                #endregion

                #region Nullable
                Argument(
                                            
                    // Название переменной, для которой передается значение
                    NameColon(
                        IdentifierName(ParseToken("nullable").WithLeadingTrivia(Whitespace("\n                "))),
                        Token(ColonToken)),
                    default,
                                            
                    // Лямбда выражение
                    column.IsNullable ?
                    LiteralExpression(
                        TrueLiteralExpression,
                        Token(TrueKeyword).WithLeadingTrivia(Whitespace(" ")))
                    :
                    LiteralExpression(
                        FalseLiteralExpression,
                        Token(FalseKeyword).WithLeadingTrivia(Whitespace(" "))))
                #endregion

                /*#region DefaultValue
                Argument(
                                            
                    // Название переменной, для которой передается значение
                    NameColon(
                        IdentifierName(ParseToken("defaultValue").WithLeadingTrivia(Whitespace("\n                "))),
                        Token(ColonToken)),
                    default,
                                            
                    // Лямбда выражение
                    LiteralExpression(
                        StringLiteralExpression, 
                        Literal(column.IsNullable).WithLeadingTrivia(Whitespace(" ")))),                                        
                #endregion*/
            };
        }

        private static IEnumerable<SyntaxNode> GetKeys(MainContext context, Table table)
        {
            foreach(TableColumn tableColumn in table.TableColumns)
            {
                if (tableColumn.IsForeignKey)
                {
                    Table foreignTable = context.Tables
                        .AsNoTracking()
                        .Include(tc => tc.TableColumns)
                        .FirstOrDefault(i => i.Id == tableColumn.ForeignTableId);
                    TableColumn foreignTableColumn = foreignTable.TableColumns.FirstOrDefault(i => i.Id == tableColumn.ForeignTableKeyId);
                    yield return ExpressionStatement(
                        InvocationExpression(
                            MemberAccessExpression(
                                SimpleMemberAccessExpression,
                                IdentifierName("table"),
                                Token(DotToken),
                                IdentifierName("ForeignKey")
                            ),
                            ArgumentList(
                                Token(OpenParenToken).WithTrailingTrivia(LineFeed),
                                SeparatedList(
                                    new List<ArgumentSyntax>()
                                    {
                                        Argument(
                                            NameColon(IdentifierName("name"), Token(ColonToken).WithTrailingTrivia(Whitespace(" "))),
                                            default,
                                            LiteralExpression(
                                                StringLiteralExpression,
                                                ParseToken($"\"FK_{table.Name}_{foreignTable.Name}_{foreignTableColumn.Name}\"")))
                                            .WithLeadingTrivia(GenerateTabs(6)),
                                        Argument(
                                            NameColon(IdentifierName("column"), Token(ColonToken).WithTrailingTrivia(Whitespace(" "))),
                                            default,
                                            SimpleLambdaExpression(Parameter(Identifier("x").WithTrailingTrivia(Whitespace(" "))))
                                                .WithArrowToken(Token(EqualsGreaterThanToken).WithTrailingTrivia(Whitespace(" ")))
                                                .WithExpressionBody(
                                                    MemberAccessExpression(
                                                        SimpleMemberAccessExpression,
                                                        IdentifierName("x"),
                                                        Token(DotToken),
                                                        IdentifierName(tableColumn.Name))))
                                            .WithLeadingTrivia(GenerateTabs(6)),
                                        Argument(
                                            NameColon(IdentifierName("principalTable"), Token(ColonToken).WithTrailingTrivia(Whitespace(" "))),
                                            default,
                                            LiteralExpression(StringLiteralExpression, ParseToken($"\"{foreignTable.Name}\"")))
                                        .WithLeadingTrivia(GenerateTabs(6)),
                                        Argument(
                                            NameColon(IdentifierName("principalColumn"), Token(ColonToken).WithTrailingTrivia(Whitespace(" "))),
                                            default,
                                            LiteralExpression(StringLiteralExpression, ParseToken($"\"{foreignTableColumn.Name}\"")))
                                        .WithLeadingTrivia(GenerateTabs(6)),
                                        Argument(
                                            NameColon(IdentifierName("onDelete"), Token(ColonToken).WithTrailingTrivia(Whitespace(" "))),
                                            default,
                                            MemberAccessExpression(
                                                SimpleMemberAccessExpression,
                                                IdentifierName("ReferentialAction"),
                                                Token(DotToken),
                                                IdentifierName(tableColumn.OnDelete)))
                                            .WithLeadingTrivia(GenerateTabs(6)),
                                        Argument(
                                            NameColon(IdentifierName("onUpdate"), Token(ColonToken).WithTrailingTrivia(Whitespace(" "))),
                                            default,
                                            MemberAccessExpression(
                                                SimpleMemberAccessExpression,
                                                IdentifierName("ReferentialAction"),
                                                Token(DotToken),
                                                IdentifierName(tableColumn.OnUpdate)))
                                            .WithLeadingTrivia(GenerateTabs(6)),
                                    },
                                    GetSeparators(5, CommaToken, LineFeed)),
                                Token(CloseParenToken))),
                        Token(SemicolonToken).WithTrailingTrivia(LineFeed))
                    .WithLeadingTrivia(GenerateTabs(5));
                }
            }
        }
    }
}
