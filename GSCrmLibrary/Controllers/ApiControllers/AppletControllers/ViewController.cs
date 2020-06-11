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
using System.Collections.Generic;
using System;
using GSCrmLibrary.Models.MainEntities;

namespace GSCrmLibrary.Controllers.ApiControllers.AppletControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViewController<TContext, TBUSFactory>
        : MainAppletController<View, BUSView, UIView, TContext, DataBUSViewFR<TContext>, TBUSFactory, BUSUIViewFR<TContext>>
        where TContext : MainContext, new()
        where TBUSFactory : BUSFactory<TContext>, new()
    {
        private readonly IViewInfo viewInfo;
        private readonly TContext context;
        public ViewController(TContext context, IScreenInfo screenInfo, IViewInfo viewInfo)
            : base(context, screenInfo, viewInfo)
        {
            this.viewInfo = viewInfo;
            this.context = context;
        }

        [HttpPost("ExecuteQuery")]
        public override void ExecuteQuery([FromBody] UIView model)
        {
            base.ExecuteQuery(model);
            string searchSpecification = "";
            List<object> searchSpecArgs = new List<object>();
            if (!string.IsNullOrWhiteSpace(model.Name))
            {
                searchSpecification += $"Name.ToLower().Contains(@0)";
                searchSpecArgs.Add(model.Name.ToLower());
            }
            if (!string.IsNullOrWhiteSpace(model.BusObjectName))
            {
                List<Guid?> objectsId = new List<Guid?>();
                context.BusinessObjects.Where(n => n.Name.ToLower().Contains(model.BusObjectName.ToLower())).ToList().ForEach(busObject => objectsId.Add(busObject.Id));
                searchSpecArgs.Add(objectsId);
                searchSpecification = string.IsNullOrWhiteSpace(searchSpecification) ? "@0.Contains(BusObjectId)" : $"{searchSpecification} && @1.Contains(BusObjectId)";
            }
            BusinessObjectComponent objectComponent = viewInfo.BOComponents.FirstOrDefault(n => n.Name == "View");
            ComponentsRecordsInfo.SetSearchSpecification(objectComponent.Name, SearchSpecTypes.SearchSpecification, searchSpecification);
            ComponentsRecordsInfo.SetSearchSpecification(objectComponent.Name, SearchSpecTypes.SearchSpecArgs, searchSpecArgs.ToArray());
        }
    }
}