using System.Linq;
using Microsoft.AspNetCore.Mvc;
using GSCrmLibrary.Data;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Models.AppletModels;
using GSCrmLibrary.Models.BusinessComponentModels;
using GSCrmLibrary.Services.Info;
using GSCrmLibrary.Factories.BUSUIFactories;
using GSCrmLibrary.Factories.DataBUSFactories;
using GSCrmLibrary.Controllers.ApiControllers.MainControllers;
using GSCrmLibrary.Factories.MainFactories;
using GSCrmLibrary.Controllers.ApiControllers.BusinessComponentControllers;
using System.Collections;
using System.Collections.Generic;
using GSCrmLibrary.Models.MainEntities;

namespace GSCrmLibrary.Controllers.ApiControllers.AppletControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TableController<TContext, TBUSFactory>
        : MainAppletController<Table, BUSTable, UITable, TContext, DataBUSTableFR<TContext>, TBUSFactory, BUSUITableFR<TContext>>
        where TContext : MainContext, new()
        where TBUSFactory : BUSFactory<TContext>, new()
    {
        private readonly IViewInfo viewInfo;
        private readonly TContext context;
        public TableController(TContext context, IScreenInfo screenInfo, IViewInfo viewInfo)
            : base(context, screenInfo, viewInfo)
        {
            this.viewInfo = viewInfo;
            this.context = context;
        }

        [HttpPost("ExecuteQuery")]
        public override void ExecuteQuery([FromBody] UITable model)
        {
            base.ExecuteQuery(model);
            string searchSpecification = "";
            List<object> searchSpecArgs = new List<object>();
            if (!string.IsNullOrWhiteSpace(model.Name))
            {
                searchSpecification += $"Name.ToLower().Contains(@0)";
                searchSpecArgs.Add(model.Name.ToLower());
            }
            BusinessObjectComponent objectComponent = viewInfo.BOComponents.FirstOrDefault(n => n.Name == "Table");
            ComponentsRecordsInfo.SetSearchSpecification(objectComponent.Name, SearchSpecTypes.SearchSpecification, searchSpecification);
            ComponentsRecordsInfo.SetSearchSpecification(objectComponent.Name, SearchSpecTypes.SearchSpecArgs, searchSpecArgs.ToArray());
        }
    }
}
