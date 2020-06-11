using System;
using System.Linq;
using GSCrmLibrary.Models.RequestModels;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Services.Info;
using GSAppContext = GSCrmApplication.Data.GSAppContext;

namespace GSCrmApplication.Factories.MainFactories
{
    public class BUSFactory : GSCrmLibrary.Factories.MainFactories.BUSFactory<GSAppContext>
    {
        public override dynamic GetRecord(Type type, GSAppContext context, IViewInfo viewInfo, BusinessComponent busComp)
            => base.GetRecord(Type.GetType("GSCrmApplication.Controllers.ApiControllers.BusinessComponentControllers." + busComp.Routing.Split("/api/")[1].Split('/')[0] + "Controller"),
                context, viewInfo, busComp);
        public override dynamic GetRecord(Type type, GSAppContext context, IViewInfo viewInfo, BusinessComponent busComp, string propertyName, string propertyValue)
            => base.GetRecord(Type.GetType("GSCrmApplication.Controllers.ApiControllers.BusinessComponentControllers." + busComp.Routing.Split("/api/")[1].Split('/')[0] + "Controller"),
                context, viewInfo, busComp, propertyName, propertyValue);
        public override void SetRecord(Type type, GSAppContext context, IViewInfo viewInfo, BusinessComponent busComp, dynamic record)
            => base.SetRecord(Type.GetType("GSCrmApplication.Controllers.ApiControllers.BusinessComponentControllers." + busComp.Routing.Split("/api/")[1].Split('/')[0] + "Controller"),
                context, viewInfo, busComp, (object)record);
        public override void InitializeComponentsRecords(Type type, GSAppContext context, IViewInfo viewInfo, FilterEntitiesModel model)
            => base.InitializeComponentsRecords(Type.GetType("GSCrmApplication.Controllers.ApiControllers.BusinessComponentControllers."
                + context.BusinessComponents.FirstOrDefault(i => i.Id == model.BusComp.Id).Routing.Split("/api/")[1].Split('/')[0] + "Controller"),
                context, viewInfo, model);
    }
}
