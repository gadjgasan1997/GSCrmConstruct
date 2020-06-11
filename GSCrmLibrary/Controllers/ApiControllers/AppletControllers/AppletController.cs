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
using System.Linq;
using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using GSCrmLibrary.Models.MainEntities;

namespace GSCrmLibrary.Controllers.ApiControllers.AppletControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppletController<TContext, TBUSFactory>
        : MainAppletController<Applet, BUSApplet, UIApplet, TContext, DataBUSAppletFR<TContext>, TBUSFactory, BUSUIAppletFR<TContext>>
        where TContext : MainContext, new()
        where TBUSFactory : BUSFactory<TContext>, new()
    {
        private readonly IViewInfo viewInfo;
        private readonly TContext context;
        public AppletController(TContext context, IScreenInfo screenInfo, IViewInfo viewInfo)
            : base (context, screenInfo, viewInfo)
        {
            this.viewInfo = viewInfo;
            this.context = context;
        }

        [HttpPost("ExecuteQuery")]
        public override void ExecuteQuery([FromBody] UIApplet model)
        {
            base.ExecuteQuery(model);
            string searchSpecification = "";
            List<object> searchSpecArgs = new List<object>();
            if (!string.IsNullOrWhiteSpace(model.Name))
            {
                searchSpecification += $"Name.ToLower().Contains(@0)";
                searchSpecArgs.Add(model.Name.ToLower());
            }
            if (!string.IsNullOrWhiteSpace(model.BusCompName))
            {
                List<Guid?> componentsId = new List<Guid?>();
                context.BusinessComponents.Where(n => n.Name.ToLower().Contains(model.BusCompName.ToLower())).ToList().ForEach(component => componentsId.Add(component.Id));
                searchSpecArgs.Add(componentsId);
                searchSpecification = string.IsNullOrWhiteSpace(searchSpecification) ? "@0.Contains(BusCompId)" : $"{searchSpecification} && {"@1.Contains(BusCompId)"}";
            }
            if (!string.IsNullOrWhiteSpace(model.Type))
            {
                searchSpecArgs.Add(model.Type.ToLower());
                string typeSearchSpec = $"Type.ToLower().Contains(@{searchSpecArgs.IndexOf(model.Type.ToLower())})";
                searchSpecification = string.IsNullOrWhiteSpace(searchSpecification) ? typeSearchSpec : $"{searchSpecification} && {typeSearchSpec}";
            }
            BusinessObjectComponent objectComponent = viewInfo.BOComponents.FirstOrDefault(n => n.Name == "Applet");
            ComponentsRecordsInfo.SetSearchSpecification(objectComponent.Name, SearchSpecTypes.SearchSpecification, searchSpecification);
            ComponentsRecordsInfo.SetSearchSpecification(objectComponent.Name, SearchSpecTypes.SearchSpecArgs, searchSpecArgs.ToArray());
        }
    }
}