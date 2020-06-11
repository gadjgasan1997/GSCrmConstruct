using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;

namespace GSCrmLibrary.Configuration
{
    public static class ApplicationConfig
    {
        private static string GSCrmConfig { get => Environment.GetEnvironmentVariable("GSCrmConfig", EnvironmentVariableTarget.Process); }
        public static string DataBaseName { get => GSCrmConfig.Split("\\D ")[1].Split("\\App ")[0].TrimStart().TrimEnd(); }
        public static string ServerName { get => GSCrmConfig.Split("\\S ")[1].Split("\\D ")[0].TrimStart().TrimEnd(); }
        public static string ApplicationName { get => GSCrmConfig.Split("\\App ")[1].TrimStart().TrimEnd(); }
        public static string SolutionDir { get => GSCrmConfig.Split("\\Root ")[1].Split("\\S ")[0].TrimStart().TrimEnd(); }
        public static string LibraryDir { get => SolutionDir + @"\GSCrmLibrary"; }
        public static string ToolsDir { get => SolutionDir + @"\GSCrmTools"; }
        public static string ApplicationsDir { get => SolutionDir + @"\GSCrmApplication"; }
        public static string DotNetCoreDir { get => Directory.GetParent(typeof(Enumerable).GetTypeInfo().Assembly.Location).FullName + Path.DirectorySeparatorChar; }
        public static string LibraryDll { get => LibraryDir + @"\bin\Debug\netcoreapp3.1\GSCrmLibrary.dll"; }
        public static string ToolsDll { get => ToolsDir + @"\bin\Debug\netcoreapp3.1\GSCrmTools.dll"; }
        public static string ApplicationsDll { get => ApplicationsDir + @"\bin\Debug\netcoreapp3.1\GSCrmApplication.dll"; }
        private static Solution GSCrmSolution { get => MSBuildWorkspace.Create().OpenSolutionAsync(SolutionDir + @"\GSCrm.sln").Result; }
        public static Project GSCrmLibrary { get => GSCrmSolution.Projects.FirstOrDefault(n => n.Name == "GSCrmLibrary"); }
        public static Project GSCrmApplication { get => GSCrmSolution.Projects.FirstOrDefault(n => n.Name == "GSCrmApplication"); }
        public static Project GSCrmTools { get => GSCrmSolution.Projects.FirstOrDefault(n => n.Name == "GSCrmTools"); }
    }
}