using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxKind;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using GSCrmLibrary.Data;
using GSCrmLibrary.Models.TableModels;
using static GSCrmLibrary.CodeGeneration.CodGenUtils;
using static GSCrmLibrary.CodeGeneration.CodGenTreesUtils;
using static GSCrmLibrary.Configuration.ApplicationConfig;
using static GSCrmLibrary.Configuration.EntitiesConfig;
using static GSCrmLibrary.Utils;

namespace GSCrmLibrary.CodeGeneration
{
    public static class CodGenController
    {
        public static void GenerateAppletCotroller(MainContext context, Guid recordId)
        {
            Applet applet = context.Applets
                .AsNoTracking()
                .Include(b => b.BusComp)
                    .ThenInclude(t => t.Table)
                .FirstOrDefault(i => i.Id == recordId);
            string permissibleTableName = GetPermissibleName(applet.BusComp?.Table.Name);
            string permissibleComponentName = GetPermissibleName(applet.BusComp?.Name);
            string permissibleAppletName = GetPermissibleName(applet.Name);
            string sysTableName = SystemPrefix + permissibleTableName + "_1";
            string sysComponentName = SystemPrefix + permissibleComponentName +"_2";
            string sysAppletName = SystemPrefix + permissibleAppletName + "_3";
            string controllerName = permissibleAppletName + "Controller";
            SyntaxTree syntaxTree = SyntaxTree(
                CompilationUnit()
                    .AddUsings(
                        UsingDirective(
                            ParseName("System.Linq").WithLeadingTrivia(Whitespace(" "))
                        ).WithTrailingTrivia(LineFeed),
                        UsingDirective(
                            ParseName("Microsoft.AspNetCore.Mvc").WithLeadingTrivia(Whitespace(" "))
                        ).WithTrailingTrivia(LineFeed),
                        UsingDirective(
                            ParseName("GSCrmApplication.Data").WithLeadingTrivia(Whitespace(" "))
                        ).WithTrailingTrivia(LineFeed),
                        UsingDirective(
                            ParseName("GSCrmApplication.Models.TableModels").WithLeadingTrivia(Whitespace(" "))
                        ).WithTrailingTrivia(LineFeed),
                        UsingDirective(
                            ParseName("GSCrmApplication.Models.AppletModels").WithLeadingTrivia(Whitespace(" "))
                        ).WithTrailingTrivia(LineFeed),
                        UsingDirective(
                            ParseName("GSCrmApplication.Models.BusinessComponentModels").WithLeadingTrivia(Whitespace(" "))
                        ).WithTrailingTrivia(LineFeed),
                        UsingDirective(
                            ParseName("GSCrmLibrary.Services.Info").WithLeadingTrivia(Whitespace(" "))
                        ).WithTrailingTrivia(LineFeed),
                        UsingDirective(
                            ParseName("GSCrmApplication.Factories.BUSUIFactories").WithLeadingTrivia(Whitespace(" "))
                        ).WithTrailingTrivia(LineFeed),
                        UsingDirective(
                            ParseName("GSCrmApplication.Factories.DataBUSFactories").WithLeadingTrivia(Whitespace(" "))
                        ).WithTrailingTrivia(LineFeed),
                        UsingDirective(
                            ParseName("GSCrmLibrary.Controllers.ApiControllers.MainControllers").WithLeadingTrivia(Whitespace(" "))
                        ).WithTrailingTrivia(LineFeed),
                        UsingDirective(
                            ParseName("GSCrmApplication.Factories.MainFactories").WithLeadingTrivia(Whitespace(" "))
                        ).WithTrailingTrivia(LineFeed),
                        UsingDirective(
                            ParseName(sysTableName + " = GSCrmApplication.Models.TableModels." + permissibleTableName)
                                .WithLeadingTrivia(Whitespace(" "))
                        ).WithTrailingTrivia(LineFeed),
                        UsingDirective(
                            ParseName(sysComponentName + " = GSCrmApplication.Models.BusinessComponentModels." + permissibleComponentName)
                                .WithLeadingTrivia(Whitespace(" "))
                        ).WithTrailingTrivia(LineFeed),
                        UsingDirective(
                            ParseName(sysAppletName + " = GSCrmApplication.Models.AppletModels." + permissibleAppletName)
                                .WithLeadingTrivia(Whitespace(" "))
                        ).WithTrailingTrivia(Whitespace("\n\n")))
                    .AddMembers(
                        NamespaceDeclaration(
                            QualifiedName(
                                QualifiedName(
                                    QualifiedName(
                                        IdentifierName("GSCrmApplication"),
                                        Token(DotToken),
                                        IdentifierName("Controllers")),
                                    Token(DotToken),
                                    IdentifierName("ApiControllers")),
                                Token(DotToken),
                                IdentifierName("AppletControllers")).WithTrailingTrivia(Whitespace("\n")))
                            .WithNamespaceKeyword(Token(NamespaceKeyword).WithTrailingTrivia(Whitespace(" ")))

                            // Открывающая скобка namespace-а
                            .WithOpenBraceToken(Token(OpenBraceToken).WithTrailingTrivia(LineFeed))

                            // Класс контроллера
                            .WithMembers(List(
                                new List<MemberDeclarationSyntax>()
                                {
                                    ClassDeclaration(ParseToken(controllerName).WithTrailingTrivia(Whitespace(" ")))
                                        .WithModifiers(
                                            TokenList(
                                                Token(PublicKeyword)
                                                    .WithLeadingTrivia(Tab)
                                                    .WithTrailingTrivia(Whitespace(" "))))
                                            .WithKeyword(Token(ClassKeyword).WithTrailingTrivia(Whitespace(" ")))
                                            
                                        // Открывающая скобка класса
                                        .WithOpenBraceToken(Token(OpenBraceToken)
                                            .WithLeadingTrivia(Tab)
                                            .WithTrailingTrivia(LineFeed))

                                        // Базовый контроллер
                                        .WithBaseList(
                                            BaseList(
                                                SingletonSeparatedList<BaseTypeSyntax>(
                                                    SimpleBaseType(
                                                        GenericName(
                                                            Identifier(GetEntityBaseType("AppletController")).WithTrailingTrivia(LineFeed),
                                                            TypeArgumentList(
                                                                Token(LessThanToken).WithLeadingTrivia(GenerateTabs(2)),
                                                                SeparatedList(
                                                                    new List<TypeSyntax>()
                                                                    {
                                                                        IdentifierName(sysTableName),
                                                                        IdentifierName(sysComponentName).WithLeadingTrivia(Whitespace(" ")),
                                                                        IdentifierName(sysAppletName).WithLeadingTrivia(Whitespace(" ")),
                                                                        IdentifierName("GSAppContext").WithLeadingTrivia(Whitespace(" ")),
                                                                        IdentifierName(DataFR + applet.BusComp?.Name + "FR").WithLeadingTrivia(Whitespace(" ")),
                                                                        IdentifierName("BUSFactory").WithLeadingTrivia(Whitespace(" ")),
                                                                        IdentifierName(UIFR + permissibleAppletName + "FR").WithLeadingTrivia(Whitespace(" "))
                                                                    },
                                                                    GetSeparators(6, CommaToken)),
                                                                Token(GreaterThanToken).WithTrailingTrivia(LineFeed)))
                                                        .WithLeadingTrivia(Whitespace(" "))))))

                                        // Атрибуты
                                        .WithAttributeLists(List(
                                            new List<AttributeListSyntax>()
                                            {
                                                // Атрибут маршрутизацией
                                                AttributeList(
                                                    Token(OpenBracketToken).WithLeadingTrivia(Tab),
                                                    default,
                                                    SeparatedList(new List<AttributeSyntax>()
                                                    {
                                                        Attribute(
                                                            IdentifierName("Route"),
                                                            AttributeArgumentList(
                                                                Token(OpenParenToken),
                                                                SeparatedList(
                                                                    new List<AttributeArgumentSyntax>()
                                                                    {
                                                                        AttributeArgument(
                                                                            LiteralExpression(
                                                                                StringLiteralExpression,
                                                                                Literal("api/[controller]"))),
                                                                    }),
                                                                Token(CloseParenToken))),
                                                    }),
                                                    Token(CloseBracketToken)).WithTrailingTrivia(LineFeed),

                                                // Атрибут api контроллера
                                                AttributeList(
                                                    Token(OpenBracketToken).WithLeadingTrivia(Tab),
                                                    default,
                                                    SeparatedList(new List<AttributeSyntax>()
                                                    {
                                                        Attribute(IdentifierName("ApiController")),
                                                    }),
                                                    Token(CloseBracketToken)).WithTrailingTrivia(LineFeed),
                                            }))
                                            
                                        // Методы класса
                                        .WithMembers(List(
                                            new List<MemberDeclarationSyntax>()
                                            {
                                                // Конструктор контроллера
                                                ConstructorDeclaration(controllerName)

                                                    // Модификатор доступа
                                                    .WithModifiers(
                                                        TokenList(
                                                            Token(PublicKeyword)
                                                                .WithLeadingTrivia(GenerateTabs(2))
                                                                .WithTrailingTrivia(Whitespace(" "))))

                                                    // Список параметров конструктора
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
                                                                            ParseTypeName("IScreenInfo")
                                                                                .WithLeadingTrivia(Whitespace(" "))
                                                                                .WithTrailingTrivia(Whitespace(" ")),
                                                                            Identifier("screenInfo"),
                                                                            default),
                                                                        Parameter(
                                                                            List(new List<AttributeListSyntax>(){ }),
                                                                            TokenList(),
                                                                            ParseTypeName("IViewInfo")
                                                                                .WithLeadingTrivia(Whitespace(" "))
                                                                                .WithTrailingTrivia(Whitespace(" ")),
                                                                            Identifier("viewInfo"),
                                                                            default)
                                                                    },
                                                                    GetSeparators(2, CommaToken)
                                                                ))
                                                            .WithCloseParenToken(Token(CloseParenToken)).WithTrailingTrivia(LineFeed))

                                                    // Вызов конструктора из базового контроллера
                                                    .WithInitializer(
                                                        ConstructorInitializer(
                                                            BaseConstructorInitializer,
                                                            Token(ColonToken)
                                                                .WithLeadingTrivia(GenerateTabs(3))
                                                                .WithTrailingTrivia(Whitespace(" ")),
                                                            Token(BaseKeyword),
                                                            ArgumentList(
                                                                Token(OpenParenToken),
                                                                SeparatedList(
                                                                    new List<ArgumentSyntax>()
                                                                    {
                                                                        Argument(IdentifierName("context")),
                                                                        Argument(IdentifierName("screenInfo")).WithLeadingTrivia(Whitespace(" ")),
                                                                        Argument(IdentifierName("viewInfo")).WithLeadingTrivia(Whitespace(" "))
                                                                    },
                                                                    GetSeparators(2, CommaToken)),
                                                                Token(CloseParenToken).WithTrailingTrivia(LineFeed))))

                                                    // Тело конструктора
                                                    .WithBody(
                                                        Block(List(new List<StatementSyntax>(){ }))

                                                            // Открывающая фигурная скобка конструктора
                                                            .WithOpenBraceToken(Token(OpenBraceToken)).WithLeadingTrivia(GenerateTabs(2))

                                                            // Закрывающая фигурная скобка конструктора
                                                            .WithCloseBraceToken(Token(CloseBraceToken)).WithTrailingTrivia(LineFeed))
                                            }))
                                        
                                        // Закрывающая скобка класса
                                        .WithCloseBraceToken(Token(CloseBraceToken)
                                            .WithLeadingTrivia(Tab)
                                            .WithTrailingTrivia(LineFeed))
                                }))

                            // Закрывающая скобка namespace-а
                            .WithCloseBraceToken(Token(CloseBraceToken))));

            // Запись и компиляция
            WriteTree(GSCrmApplication, controllerName, "AppletController", syntaxTree);
            Compile("GSCrm" + "AppletController", syntaxTree);
        }

        public static void GenerateComponentCotroller(MainContext context, Guid recordId)
        {
            BusinessComponent component = context.BusinessComponents
                .AsNoTracking()
                .Include(b => b.Table)
                .FirstOrDefault(i => i.Id == recordId);
            string permissibleTableName = GetPermissibleName(component.Table?.Name);
            string permissibleComponentName = GetPermissibleName(component.Name);
            string sysTableName = SystemPrefix + permissibleTableName;
            string sysComponentName = SystemPrefix + permissibleComponentName;
            if (sysTableName == sysComponentName)
            {
                sysTableName += "_1";
                sysComponentName += "_2";
            }
            string controllerName = permissibleComponentName + "Controller";
            SyntaxTree syntaxTree = SyntaxTree(
                CompilationUnit()
                    .AddUsings(
                        UsingDirective(
                            ParseName("System.Linq").WithLeadingTrivia(Whitespace(" "))
                        ).WithTrailingTrivia(LineFeed),
                        UsingDirective(
                            ParseName("Microsoft.AspNetCore.Mvc").WithLeadingTrivia(Whitespace(" "))
                        ).WithTrailingTrivia(LineFeed),
                        UsingDirective(
                            ParseName("GSCrmApplication.Data").WithLeadingTrivia(Whitespace(" "))
                        ).WithTrailingTrivia(LineFeed),
                        UsingDirective(
                            ParseName("GSCrmApplication.Models.TableModels").WithLeadingTrivia(Whitespace(" "))
                        ).WithTrailingTrivia(LineFeed),
                        UsingDirective(
                            ParseName("GSCrmApplication.Models.AppletModels").WithLeadingTrivia(Whitespace(" "))
                        ).WithTrailingTrivia(LineFeed),
                        UsingDirective(
                            ParseName("GSCrmApplication.Models.BusinessComponentModels").WithLeadingTrivia(Whitespace(" "))
                        ).WithTrailingTrivia(LineFeed),
                        UsingDirective(
                            ParseName("GSCrmLibrary.Services.Info").WithLeadingTrivia(Whitespace(" "))
                        ).WithTrailingTrivia(LineFeed),
                        UsingDirective(
                            ParseName("GSCrmApplication.Factories.DataBUSFactories").WithLeadingTrivia(Whitespace(" "))
                        ).WithTrailingTrivia(LineFeed),
                        UsingDirective(
                            ParseName("GSCrmLibrary.Controllers.ApiControllers.MainControllers").WithLeadingTrivia(Whitespace(" "))
                        ).WithTrailingTrivia(LineFeed),
                        UsingDirective(
                            ParseName("GSCrmApplication.Factories.MainFactories").WithLeadingTrivia(Whitespace(" "))
                        ).WithTrailingTrivia(LineFeed),
                        UsingDirective(
                            ParseName(sysTableName + " = GSCrmApplication.Models.TableModels." + permissibleTableName)
                                .WithLeadingTrivia(Whitespace(" "))
                        ).WithTrailingTrivia(LineFeed),
                        UsingDirective(
                            ParseName(sysComponentName + " = GSCrmApplication.Models.BusinessComponentModels." + permissibleComponentName)
                                .WithLeadingTrivia(Whitespace(" "))
                        ).WithTrailingTrivia(Whitespace("\n\n")))
                    .AddMembers(
                        NamespaceDeclaration(
                            QualifiedName(
                                QualifiedName(
                                    QualifiedName(
                                        IdentifierName("GSCrmApplication"),
                                        Token(DotToken),
                                        IdentifierName("Controllers")),
                                    Token(DotToken),
                                    IdentifierName("ApiControllers")),
                                Token(DotToken),
                                IdentifierName("BusinessComponentControllers")).WithTrailingTrivia(Whitespace("\n")))
                            .WithNamespaceKeyword(Token(NamespaceKeyword).WithTrailingTrivia(Whitespace(" ")))

                            // Открывающая скобка namespace-а
                            .WithOpenBraceToken(Token(OpenBraceToken).WithTrailingTrivia(LineFeed))

                            // Класс контроллера
                            .WithMembers(List(
                                new List<MemberDeclarationSyntax>()
                                {
                                    ClassDeclaration(ParseToken(controllerName).WithTrailingTrivia(Whitespace(" ")))
                                        .WithModifiers(
                                            TokenList(
                                                Token(PublicKeyword)
                                                    .WithLeadingTrivia(Tab)
                                                    .WithTrailingTrivia(Whitespace(" "))))
                                            .WithKeyword(Token(ClassKeyword).WithTrailingTrivia(Whitespace(" ")))
                                            
                                        // Открывающая скобка класса
                                        .WithOpenBraceToken(Token(OpenBraceToken)
                                            .WithLeadingTrivia(Tab)
                                            .WithTrailingTrivia(LineFeed))

                                        // Базовый контроллер
                                        .WithBaseList(
                                            BaseList(
                                                SingletonSeparatedList<BaseTypeSyntax>(
                                                    SimpleBaseType(
                                                        GenericName(
                                                            Identifier(GetEntityBaseType("BusinessComponentController")).WithTrailingTrivia(LineFeed),
                                                            TypeArgumentList(
                                                                Token(LessThanToken).WithLeadingTrivia(GenerateTabs(2)),
                                                                SeparatedList(
                                                                    new List<TypeSyntax>()
                                                                    {
                                                                        IdentifierName(sysTableName),
                                                                        IdentifierName(sysComponentName).WithLeadingTrivia(Whitespace(" ")),
                                                                        IdentifierName("GSAppContext").WithLeadingTrivia(Whitespace(" ")),
                                                                        IdentifierName(DataFR + component.Name + "FR").WithLeadingTrivia(Whitespace(" ")),
                                                                        IdentifierName("BUSFactory").WithLeadingTrivia(Whitespace(" "))
                                                                    },
                                                                    GetSeparators(4, CommaToken)),
                                                                Token(GreaterThanToken).WithTrailingTrivia(LineFeed)))
                                                        .WithLeadingTrivia(Whitespace(" "))))))

                                        // Атрибуты
                                        .WithAttributeLists(List(
                                            new List<AttributeListSyntax>()
                                            {
                                                // Атрибут маршрутизацией
                                                AttributeList(
                                                    Token(OpenBracketToken).WithLeadingTrivia(Tab),
                                                    default,
                                                    SeparatedList(new List<AttributeSyntax>()
                                                    {
                                                        Attribute(
                                                            IdentifierName("Route"),
                                                            AttributeArgumentList(
                                                                Token(OpenParenToken),
                                                                SeparatedList(
                                                                    new List<AttributeArgumentSyntax>()
                                                                    {
                                                                        AttributeArgument(
                                                                            LiteralExpression(
                                                                                StringLiteralExpression,
                                                                                Literal("api/[controller]"))),
                                                                    }),
                                                                Token(CloseParenToken))),
                                                    }),
                                                    Token(CloseBracketToken)).WithTrailingTrivia(LineFeed),

                                                // Атрибут api контроллера
                                                AttributeList(
                                                    Token(OpenBracketToken).WithLeadingTrivia(Tab),
                                                    default,
                                                    SeparatedList(new List<AttributeSyntax>()
                                                    {
                                                        Attribute(IdentifierName("ApiController")),
                                                    }),
                                                    Token(CloseBracketToken)).WithTrailingTrivia(LineFeed),
                                            }))
                                            
                                        // Методы класса
                                        .WithMembers(List(
                                            new List<MemberDeclarationSyntax>()
                                            {
                                                // Конструктор контроллера
                                                ConstructorDeclaration(controllerName)

                                                    // Модификатор доступа
                                                    .WithModifiers(
                                                        TokenList(
                                                            Token(PublicKeyword)
                                                                .WithLeadingTrivia(GenerateTabs(2))
                                                                .WithTrailingTrivia(Whitespace(" "))))

                                                    // Список параметров конструктора
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
                                                                            default)
                                                                    },
                                                                    GetSeparators(1, CommaToken)
                                                                ))
                                                            .WithCloseParenToken(Token(CloseParenToken)).WithTrailingTrivia(LineFeed))

                                                    // Вызов конструктора из базового контроллера
                                                    .WithInitializer(
                                                        ConstructorInitializer(
                                                            BaseConstructorInitializer,
                                                            Token(ColonToken)
                                                                .WithLeadingTrivia(GenerateTabs(3))
                                                                .WithTrailingTrivia(Whitespace(" ")),
                                                            Token(BaseKeyword),
                                                            ArgumentList(
                                                                Token(OpenParenToken),
                                                                SeparatedList(
                                                                    new List<ArgumentSyntax>()
                                                                    {
                                                                        Argument(IdentifierName("context")),
                                                                        Argument(IdentifierName("viewInfo")).WithLeadingTrivia(Whitespace(" "))
                                                                    },
                                                                    GetSeparators(1, CommaToken)),
                                                                Token(CloseParenToken).WithTrailingTrivia(LineFeed))))

                                                    // Тело конструктора
                                                    .WithBody(
                                                        Block(List(new List<StatementSyntax>(){ }))

                                                            // Открывающая фигурная скобка конструктора
                                                            .WithOpenBraceToken(Token(OpenBraceToken)).WithLeadingTrivia(GenerateTabs(2))

                                                            // Закрывающая фигурная скобка конструктора
                                                            .WithCloseBraceToken(Token(CloseBraceToken)).WithTrailingTrivia(LineFeed))
                                            }))
                                        
                                        // Закрывающая скобка класса
                                        .WithCloseBraceToken(Token(CloseBraceToken)
                                            .WithLeadingTrivia(Tab)
                                            .WithTrailingTrivia(LineFeed))
                                }))

                            // Закрывающая скобка namespace-а
                            .WithCloseBraceToken(Token(CloseBraceToken))));

            // Запись и компиляция
            WriteTree(GSCrmApplication, controllerName, "BusinessComponentController", syntaxTree);
            Compile("GSCrm" + "BusinessComponentController", syntaxTree);
        }

        public static void GenerateScreenController(MainContext context, Guid recordId)
        {
            Screen screen = context.Screens.AsNoTracking().FirstOrDefault(i => i.Id == recordId);
            string permissibleScreenName = GetPermissibleName(screen.Name);
            string controllerName = permissibleScreenName + "Controller";
            SyntaxTree syntaxTree = SyntaxTree(
                CompilationUnit()
                    .AddUsings(
                        UsingDirective(
                            ParseName("Microsoft.AspNetCore.Mvc").WithLeadingTrivia(Whitespace(" "))
                        ).WithTrailingTrivia(Whitespace("\n\n")))
                    .AddMembers(
                        NamespaceDeclaration(
                            QualifiedName(
                                QualifiedName(
                                    IdentifierName("GSCrmApplication"),
                                    Token(DotToken),
                                    IdentifierName("Controllers")),
                                Token(DotToken),
                                IdentifierName("MVCControllers")).WithTrailingTrivia(Whitespace("\n")))
                            .WithNamespaceKeyword(Token(NamespaceKeyword).WithTrailingTrivia(Whitespace(" ")))

                            // Открывающая скобка namespace-а
                            .WithOpenBraceToken(Token(OpenBraceToken).WithTrailingTrivia(LineFeed))

                            // Класс
                            .WithMembers(List(
                                new List<MemberDeclarationSyntax>()
                                {
                                    ClassDeclaration(ParseToken(controllerName).WithTrailingTrivia(Whitespace(" ")))

                                        // Ключевое слово класса
                                        .WithKeyword(Token(ClassKeyword).WithTrailingTrivia(Whitespace(" ")))
                                    
                                        // Модификатор доступа
                                        .WithModifiers(TokenList(
                                            Token(PublicKeyword)
                                                .WithTrailingTrivia(Whitespace(" "))
                                                .WithLeadingTrivia(Tab)))

                                        // Базовый контроллер
                                        .WithBaseList(BaseList(
                                            SingletonSeparatedList<BaseTypeSyntax>(
                                                SimpleBaseType(IdentifierName("Controller"))
                                                    .WithLeadingTrivia(Whitespace(" "))
                                                    .WithTrailingTrivia(LineFeed))))

                                        // Открывающая скобка класса
                                        .WithOpenBraceToken(Token(OpenBraceToken)
                                            .WithLeadingTrivia(Tab)
                                            .WithTrailingTrivia(LineFeed))

                                        // Методы класса
                                        .WithMembers(List(
                                            new List<MemberDeclarationSyntax>()
                                            {
                                                MethodDeclaration(ParseTypeName("IActionResult").WithTrailingTrivia(Whitespace(" ")), "Index")
                                                
                                                    // Модификатор доступа
                                                    .WithModifiers(TokenList(
                                                        Token(PublicKeyword)
                                                            .WithTrailingTrivia(Whitespace(" "))
                                                            .WithLeadingTrivia(GenerateTabs(2))))
                                                    
                                                    // Список параметров
                                                    .WithParameterList(
                                                        ParameterList()
                                                            .WithOpenParenToken(Token(OpenParenToken))
                                                            .WithParameters(SeparatedList( new List<ParameterSyntax>() { }))
                                                            .WithCloseParenToken(Token(CloseParenToken)).WithTrailingTrivia(Whitespace(" ")))

                                                    // Атрибуты
                                                    .WithAttributeLists(List(
                                                        new List<AttributeListSyntax>()
                                                        {
                                                            // Атрибут с маршрутизацией
                                                            AttributeList(
                                                                Token(OpenBracketToken).WithLeadingTrivia(GenerateTabs(2)),
                                                                default,
                                                                SeparatedList(new List<AttributeSyntax>()
                                                                {
                                                                    Attribute(
                                                                        IdentifierName("Route"),
                                                                        AttributeArgumentList(
                                                                            Token(OpenParenToken),
                                                                            SeparatedList(
                                                                                new List<AttributeArgumentSyntax>()
                                                                                {
                                                                                    AttributeArgument(LiteralExpression(StringLiteralExpression, Literal($"{screen.Name}"))),
                                                                                }),
                                                                            Token(CloseParenToken))),
                                                                }),
                                                                Token(CloseBracketToken)).WithTrailingTrivia(LineFeed)
                                                        }))

                                                    // Лямбда выражение
                                                    .WithExpressionBody(
                                                        ArrowExpressionClause(
                                                            Token(EqualsGreaterThanToken).WithTrailingTrivia(Whitespace(" ")),
                                                            InvocationExpression(
                                                                IdentifierName("View"),
                                                                ArgumentList(
                                                                    Token(OpenParenToken),
                                                                    SeparatedList(List(new List<ArgumentSyntax>(){ })),
                                                                    Token(CloseParenToken)))))
                                                        .WithSemicolonToken(Token(SemicolonToken).WithTrailingTrivia(LineFeed))
                                                        .WithTrailingTrivia(LineFeed)
                                            }))

                                        // Закрывающая скобка класса
                                        .WithCloseBraceToken(Token(CloseBraceToken)
                                            .WithLeadingTrivia(Tab)
                                            .WithTrailingTrivia(LineFeed))
                                }))));

            // Запись и компиляция
            string directoryPath = $@"{ApplicationsDir}\Views\{permissibleScreenName}";
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);
            using (var stream = File.OpenWrite($"{directoryPath}\\Index.cshtml"))
            {
                stream.Seek(0, SeekOrigin.Begin);
                stream.Write(new UTF8Encoding(true).GetBytes($"@{{ViewData[\"Title\"] = \"{screen.Name} Page\";}}"));
            }
            WriteTree(GSCrmApplication, controllerName, "ScreenController", syntaxTree);
            Compile("GSCrm" + "ScreenController", syntaxTree);
        }
    }
}