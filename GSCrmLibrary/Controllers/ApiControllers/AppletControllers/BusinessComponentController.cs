using System.Linq;
using System.Collections;
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
using System.Collections.Generic;
using System;
using GSCrmLibrary.Models.MainEntities;

namespace GSCrmLibrary.Controllers.ApiControllers.AppletControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessComponentController<TContext, TBUSFactory>
        : MainAppletController<BusinessComponent, BUSBusinessComponent, UIBusinessComponent, TContext, DataBUSBusCompFR<TContext>, TBUSFactory, BUSUIBusCompFR<TContext>>
        where TContext : MainContext, new()
        where TBUSFactory : BUSFactory<TContext>, new()
    {
        private readonly IViewInfo viewInfo;
        private readonly TContext context;
        public BusinessComponentController(TContext context, IScreenInfo screenInfo, IViewInfo viewInfo)
            : base(context, screenInfo, viewInfo)
        {
            this.viewInfo = viewInfo;
            this.context = context;
        }

        [HttpPost("ExecuteQuery")]
        public override void ExecuteQuery([FromBody] UIBusinessComponent model)
        {
            base.ExecuteQuery(model);
            string searchSpecification = "";
            List<object> searchSpecArgs = new List<object>();
            if (!string.IsNullOrWhiteSpace(model.Name))
            {
                searchSpecification += $"Name.ToLower().Contains(@0)";
                searchSpecArgs.Add(model.Name.ToLower());
            }
            if (!string.IsNullOrWhiteSpace(model.TableName))
            {
                List<Guid?> tablesId = new List<Guid?>();
                context.Tables.Where(n => n.Name.ToLower().Contains(model.TableName.ToLower())).ToList().ForEach(table => tablesId.Add(table.Id));
                searchSpecArgs.Add(tablesId);
                searchSpecification = string.IsNullOrWhiteSpace(searchSpecification) ? "@0.Contains(TableId)" : $"{searchSpecification} && @1.Contains(TableId)";
            }
            BusinessObjectComponent objectComponent = viewInfo.BOComponents.FirstOrDefault(n => n.Name == "Business Component");
            ComponentsRecordsInfo.SetSearchSpecification(objectComponent.Name, SearchSpecTypes.SearchSpecification, searchSpecification);
            ComponentsRecordsInfo.SetSearchSpecification(objectComponent.Name, SearchSpecTypes.SearchSpecArgs, searchSpecArgs.ToArray());
        }
    }
}