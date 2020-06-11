using System.Collections.Generic;
using GSCrmLibrary.Models.MainEntities;

namespace GSCrmLibrary.Data
{
    public static class ComponentsContext<TBusinessComponent>
        where TBusinessComponent : IBUSEntity, new()
    {
        private static Dictionary<string, TBusinessComponent> componentsContext { get; set; }
        private static List<TBusinessComponent> pickListContext { get; set; }
        public static Dictionary<string, TBusinessComponent> GetComponentsContext()
        {
            if (componentsContext == null)
                componentsContext = new Dictionary<string, TBusinessComponent>();
            return componentsContext;
        }
        public static bool TryGetComponentContext(string componentName, out TBusinessComponent businessComponent)
        {
            if (componentsContext == null)
                componentsContext = new Dictionary<string, TBusinessComponent>();
            if (componentsContext.ContainsKey(componentName) && componentsContext.GetValueOrDefault(componentName) != null)
            {
                businessComponent = componentsContext[componentName];
                return true;
            }
            else
            {
                businessComponent = new TBusinessComponent() { };
                return false;
            }
        }
        public static void SetComponentContext(string componentName, TBusinessComponent componentContext)
        {
            if (componentsContext == null)
                componentsContext = new Dictionary<string, TBusinessComponent>();
            if (componentsContext.ContainsKey(componentName))
                componentsContext[componentName] = componentContext;
            else componentsContext.Add(componentName, componentContext);
        }
        public static List<TBusinessComponent> GetPickListContext()
            => pickListContext;
        public static void SetPickListContext(List<TBusinessComponent> businessEntities)
            => pickListContext = businessEntities;
        public static void ClearPickListContext() => pickListContext = new List<TBusinessComponent>();
        public static void RemoveComponentContext(string componentName)
        {
            if (componentsContext.ContainsKey(componentName))
                componentsContext.Remove(componentName);
        }
        public static void Dispose() => componentsContext = new Dictionary<string, TBusinessComponent>();
    }
}