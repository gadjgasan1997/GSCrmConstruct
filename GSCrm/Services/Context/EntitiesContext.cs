using GSCrm.Data;
using GSCrm.Models.Default.MainEntities;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace GSCrm.Services.Context
{
    public class EntitiesContext : IEntitiesContext
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly ApplicationContext context;
        public EntitiesContext(IServiceScopeFactory _scopeFactory)
        {
            scopeFactory = _scopeFactory;
            context = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<ApplicationContext>();
        }

        // Названия бизнес компонент и их котекст
        public Dictionary<string, IEnumerable<MainTable>> ContextDictionary { get => new Dictionary<string, IEnumerable<MainTable>>()
        {
            { "Applet", context.Applets },
            { "Business Component", context.BusinessComponents },
            { "Business Object Component", context.BusinessObjectComponents },
            { "Business Object", context.BusinessObjects },
            { "Column", context.Columns },
            { "Control", context.Controls },
            { "Field", context.Fields },
            { "Link", context.Links },
            { "View Item", context.ViewItems },
            { "View", context.Views },
            { "PickList", context.PickLists },
            { "Icon", context.Icons },
            { "Physical Render", context.PhysicalRenders },
            { "Action", context.Actions },
            { "Action UP", context.ActionUPs },
            { "Control UP", context.ControlUPs },
            { "Table", context.Tables },
            { "Table Column", context.TableColumns },
        }; set { ContextDictionary = value; } }
    }
}
