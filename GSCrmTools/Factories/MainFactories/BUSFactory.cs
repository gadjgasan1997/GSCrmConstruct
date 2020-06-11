using System;
using System.Linq;
using GSCrmLibrary.Models.RequestModels;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Services.Info;
using GSCrmTools.Data;

namespace GSCrmTools.Factories.MainFactories
{
    public class BUSFactory : GSCrmLibrary.Factories.MainFactories.BUSFactory<ToolsContext>
    {
        public override dynamic GetRecord(Type type, ToolsContext context, IViewInfo viewInfo, BusinessComponent busComp)
            => base.GetRecord(Type.GetType("GSCrmTools.Controllers.ApiControllers.BusinessComponentControllers." + busComp.Routing.Split("/api/")[1].Split('/')[0] + "Controller"),
                context, viewInfo, busComp);
        public override dynamic GetRecord(Type type, ToolsContext context, IViewInfo viewInfo, BusinessComponent busComp, string propertyName, string propertValue)
            => base.GetRecord(Type.GetType("GSCrmTools.Controllers.ApiControllers.BusinessComponentControllers." + busComp.Routing.Split("/api/")[1].Split('/')[0] + "Controller"),
                context, viewInfo, busComp, propertyName, propertValue);
        public override void SetRecord(Type type, ToolsContext context, IViewInfo viewInfo, BusinessComponent busComp, dynamic record)
            => base.SetRecord(Type.GetType("GSCrmTools.Controllers.ApiControllers.BusinessComponentControllers." + busComp.Routing.Split("/api/")[1].Split('/')[0] + "Controller"),
                context, viewInfo, busComp, (object)record);
        public override void InitializeComponentsRecords(Type type, ToolsContext context, IViewInfo viewInfo, FilterEntitiesModel model)
            => base.InitializeComponentsRecords(Type.GetType("GSCrmTools.Controllers.ApiControllers.BusinessComponentControllers."
                + context.BusinessComponents.FirstOrDefault(i => i.Id == model.BusComp.Id).Routing.Split("/api/")[1].Split('/')[0] + "Controller"),
                context, viewInfo, model);
    }
}