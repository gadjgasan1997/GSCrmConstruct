using System;
using System.Reflection;
using System.IO;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using GSCrm.Data;
using Microsoft.EntityFrameworkCore;

namespace GSCrm.CodeDOM.Default
{
    public class CodeDOMTable
    {
        CodeCompileUnit targetUnit;
        CodeTypeDeclaration tableClass;
        
        public void CreateTable(string fileName, string className)
        {
            // Создание класса таблицы
            /*targetUnit = new CodeCompileUnit();
            CodeNamespace samples = new CodeNamespace("GSCrm.Models.Default.TableModels");
            samples.Imports.Add(new CodeNamespaceImport("GSCrm.Models.Default.MainEntities"));
            samples.Imports.Add(new CodeNamespaceImport("System.Collections.Generic"));
            tableClass = new CodeTypeDeclaration(className + " : MainTable");
            tableClass.IsClass = true;
            tableClass.TypeAttributes = TypeAttributes.Public;
            samples.Types.Add(tableClass);
            targetUnit.Namespaces.Add(samples);
            
            // Запись класса таблциы
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CodeGeneratorOptions options = new CodeGeneratorOptions();
            options.BracingStyle = "C";
            using (StreamWriter sourceWriter = new StreamWriter(fileName))
                provider.GenerateCodeFromCompileUnit(targetUnit, sourceWriter, options);

            // Запись свойства
            List<string> content = new List<string>();
            using (StreamReader sourceReader = new StreamReader(@"C:\Users\Гасан\source\repos\GSCrm\GSCrm\Data\Context\ApplicationContext.cs"))
            {
                string line;
                while ((line = sourceReader.ReadLine()) != null)
                    content.Add(line);
            }
            using (StreamWriter sourceWriter = new StreamWriter(@"C:\Users\Гасан\source\repos\GSCrm\GSCrm\Data\Context\ApplicationContext.cs"))
            {
                List<string> newContent = new List<string>();
                newContent.AddRange(content);
                foreach (var line in content)
                {
                    if (line.Contains("public class ApplicationContext : DbContext"))
                    {
                        sourceWriter.WriteLine(line);
                        sourceWriter.WriteLine("    {");
                        newContent.Remove(line);
                        newContent.RemoveAt(0);
                        break;
                    }
                    sourceWriter.WriteLine(line);
                    newContent.Remove(line);
                }
                sourceWriter.WriteLine("        public DbSet<" + className + "> " + className + "s { get; set; }");
                newContent.ForEach(line => sourceWriter.WriteLine(line));
            }*/
        }
    }
}
