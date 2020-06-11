using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Microsoft.CodeAnalysis;
using static GSCrmLibrary.Configuration.ApplicationConfig;

namespace GSCrmLibrary.Configuration
{
    public static class EntitiesConfig
    {
        public const string SystemPrefix = "Sys";
        public const string Data = "Data";
        public const string BUS = "BUS";
        public const string UI = "UI";
        public const string DataFR = Data + BUS;
        public const string UIFR = BUS + UI;
        private static EntitiesConfigModel GetEntitiesConfig()
        {
            EntitiesConfigModel model = new EntitiesConfigModel();
            using (var stream = new StreamReader(LibraryDir + @"\Configuration\entitiesconfig.json"))
                model = JsonConvert.DeserializeObject<EntitiesConfigModel>(stream.ReadToEnd());
            return model;
        }

        public static string GetEntityNamespace(string entityType)
        {
            EntitiesConfigModel config = GetEntitiesConfig();
            return entityType switch
            {
                "Table" => config.Table.Namespace,
                "BusinessComponent" => config.BusinessComponent.Namespace,
                "Applet" => config.Applet.Namespace,
                "DataBUSFactory" => config.DataBUSFactory.Namespace,
                "BUSUIFactory" => config.BUSUIFactory.Namespace,
                "AppletController" => config.AppletController.Namespace,
                "BusinessComponentController" => config.BusinessComponentController.Namespace,
                "ScreenController" => config.ScreenController.Namespace,
                "Context" => config.Context.Path,
                "Dll" => config.Dll.Path,
                "Migration" => config.Migration.Path,
                _ => config.Table.Namespace,
            };
        }

        public static string GetEntityPath(string entityType)
        {
            EntitiesConfigModel config = GetEntitiesConfig();
            return entityType switch
            {
                "Table" => config.Table.Path,
                "BusinessComponent" => config.BusinessComponent.Path,
                "Applet" => config.Applet.Path,
                "DataBUSFactory" => config.DataBUSFactory.Path,
                "BUSUIFactory" => config.BUSUIFactory.Path,
                "AppletController" => config.AppletController.Path,
                "BusinessComponentController" => config.BusinessComponentController.Path,
                "ScreenController" => config.ScreenController.Path,
                "ViewCshtml" => config.ViewCshtml.Path,
                "Context" => config.Context.Path,
                "Dll" => config.Dll.Path,
                "Migration" => config.Migration.Path,
                _ => config.Context.Path,
            };
        }

        public static string GetEntityBaseType(string entityType)
        {
            EntitiesConfigModel config = GetEntitiesConfig();
            return entityType switch
            {
                "Table" => config.Table.BaseType,
                "BusinessComponent" => config.BusinessComponent.BaseType,
                "Applet" => config.Applet.BaseType,
                "DataBUSFactory" => config.DataBUSFactory.BaseType,
                "BUSUIFactory" => config.BUSUIFactory.BaseType,
                "AppletController" => config.AppletController.BaseType,
                "BusinessComponentController" => config.BusinessComponentController.BaseType,
                _ => config.Table.BaseType,
            };
        }

        public static string GetEntityFullPath(Project project, string entityName, string entityType)
        {
            string entityPath = GetEntityPath(entityType);
            return project.Name == GSCrmTools.Name ? ToolsDir + entityPath + entityName : ApplicationsDir + entityPath + entityName;
        }

        public static string GetEntityExPath(Project project, string entityName, string entityType)
        {
            string fullPath = GetEntityFullPath(project, entityName, entityType);
            if (entityType == "Dll")
                fullPath += ".dll";
            else fullPath += ".cs";
            return fullPath;
        }

        public static string GetEntityDirectoryPath(Project project, string entityName, string entityType)
        {
            string fullPath = GetEntityFullPath(project, entityName, entityType);
            fullPath = fullPath.Replace(entityName, "");
            return fullPath;
        }
    }
}
