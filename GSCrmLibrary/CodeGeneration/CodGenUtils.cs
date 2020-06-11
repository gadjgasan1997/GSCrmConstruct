using System;
using System.IO;
using System.Text.Json;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using Migration = Microsoft.EntityFrameworkCore.Migrations.Migration;
using static GSCrmLibrary.Configuration.EntitiesConfig;
using static GSCrmLibrary.Configuration.ApplicationConfig;
using System.ComponentModel.DataAnnotations;
using static GSCrmLibrary.Utils;

namespace GSCrmLibrary.CodeGeneration
{
    public static class CodGenUtils
    {
        private static List<CSharpCompilation> Compilations { get; set; } = new List<CSharpCompilation>();

        public static SyntaxTree GetDocumentTree(Project project, string path)
        {
            Document document = project.Documents.FirstOrDefault(p => p.FilePath == path);
            return document switch
            {
                null => null,
                _ => document.GetSyntaxTreeAsync().Result
            };
        }

        public static SyntaxTree GetSyntaxTree(Project project, string entityName, string entityType, IEnumerable<SyntaxTree> syntaxTrees)
        {
            string path = GetEntityExPath(project, entityName, entityType);
            return path switch
            {
                null => null,
                _ => syntaxTrees.FirstOrDefault(n => n.FilePath == path)
            };
        }

        public static Compilation GetFreshEntityCompilation(Project project, string entityName, string entityType)
        {
            CSharpCompilation compilation = GetCompilation("GSCrm" + entityType);
            if (compilation != null && GetSyntaxTree(project, entityName, entityType, compilation.SyntaxTrees) != null)
                return compilation;
            else return GSCrmTools.GetCompilationAsync().Result;
        }

        public static SyntaxTree GetFreshEntitySyntaxTree(Project project, string entityName, string entityType)
        {
            CSharpCompilation compilation = GetCompilation("GSCrm" + entityType);
            SyntaxTree syntaxTree = null;
            if (compilation != null)
                syntaxTree = GetSyntaxTree(project, entityName, entityType, compilation.SyntaxTrees);
            if (syntaxTree == null)
            {
                string path = GetEntityExPath(project, entityName, entityType);
                if (path != null) syntaxTree = GetDocumentTree(project, path);
            }
            
            return syntaxTree;
        }

        public static bool TryGetModel(Project project, string entityName, string entityType, out SyntaxTree syntaxTree)
        {
            syntaxTree = GetFreshEntitySyntaxTree(project, entityName, entityType);
            return syntaxTree switch
            {
                null => false,
                _ => true
            };
        }

        public static CSharpCompilation Compile(string assemblyName, SyntaxTree syntaxTree)
        {
            CSharpCompilation compilation = Compilations.FirstOrDefault(n => n.AssemblyName == assemblyName);
            if (compilation == null)
            {
                compilation = CSharpCompilation.Create(
                assemblyName,
                new List<SyntaxTree>() { syntaxTree },
                new List<MetadataReference>()
                {
                    MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                    MetadataReference.CreateFromFile(typeof(JsonSerializer).Assembly.Location),
                    MetadataReference.CreateFromFile(ToolsDll),
                    //MetadataReference.CreateFromFile(ApplicationsDll),
                    MetadataReference.CreateFromFile(LibraryDll),
                    MetadataReference.CreateFromFile(typeof(DbLoggerCategory.Database).Assembly.Location),
                    MetadataReference.CreateFromFile(typeof(Migration).Assembly.Location),
                    MetadataReference.CreateFromFile(typeof(Expression).Assembly.Location),
                    MetadataReference.CreateFromFile(typeof(DisplayAttribute).Assembly.Location),
                    MetadataReference.CreateFromFile(DotNetCoreDir + "System.Runtime.dll"),
                    MetadataReference.CreateFromFile(DotNetCoreDir + "netstandard.dll")
                },
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

                Compilations.Add(compilation);
            }

            else
            {
                SyntaxTree oldTree = compilation.SyntaxTrees.FirstOrDefault(p => p.FilePath == syntaxTree.FilePath);
                if (oldTree != null)
                {
                    Compilations.Remove(compilation);
                    compilation = compilation.ReplaceSyntaxTree(oldTree, syntaxTree);
                    Compilations.Add(compilation);
                }
                else
                {
                    Compilations.Remove(compilation);
                    compilation = compilation.AddSyntaxTrees(syntaxTree);
                    Compilations.Add(compilation);
                }
            }

            return compilation;
        }

        public static CSharpCompilation GetCompilation(string assemblyName)
            => Compilations.FirstOrDefault(n => n.Assembly.Name == assemblyName);

        public static void WriteCompilation(Project project, string assemblyName)
        {
            string path = GetEntityExPath(project, assemblyName, "Dll");
            using var stream = File.OpenWrite(path);
            Compilations.FirstOrDefault(n => n.AssemblyName == assemblyName).Emit(stream);
        }

        public static void WriteTree(Project project, string entityName, string entityType, SyntaxTree syntaxTree)
        {
            string path = GetEntityExPath(project, GetPermissibleName(entityName), entityType);
            using StreamWriter streamWriter = new StreamWriter(path);
            streamWriter.Write(syntaxTree);
        }

        public static bool IsSystemProperty(string propertyName)
        {
            if (new string[] { "Id", "Created", "CreatedBy", "LastUpdated", "UpdatedBy" }.Contains(propertyName))
                return true;
            else return false;
        }

        public static string ConvertFromAnotherType(string type)
        {
            return type switch
            {
                "bool" => "Convert.ToBoolean",
                "string" => "Convert.ToString",
                "int" => "Convert.ToInt32",
                "double" => "Convert.ToDouble",
                "decimal" => "Convert.ToDecimal",
                "char" => "Convert.ToChar",
                _ => "Convert.ToString"
            };
        }

        public static void DeleteEntityFile(string entityName, string entityType)
            => File.Delete(GetEntityExPath(GSCrmApplication, entityName, entityType));

        public static bool EntityFileExists(string entityName, string entityType)
            => File.Exists(GetEntityExPath(GSCrmApplication, entityName, entityType));

        public static void DeleteEntityDirectory(string entityName, string entityType)
            => Directory.Delete(GetEntityDirectoryPath(GSCrmApplication, entityName, entityType));
    }
}