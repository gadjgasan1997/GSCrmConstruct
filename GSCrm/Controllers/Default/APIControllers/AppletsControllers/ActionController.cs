using GSCrm.Data;
using GSCrm.Models.Default.AppletModels;
using GSCrm.Models.Default.BusinessComponentModels;
using GSCrm.Services.Info;
using Action = GSCrm.Models.Default.TableModels.Action;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using GSCrm.Factories.Default.BUSUIFactories;
using GSCrm.Factories.Default.DataBUSFactories;
using GSCrm.Controllers.Default.APIControllers.MainControllers;

namespace GSCrm.Controllers.Default.APIControllers.AppletsControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActionController
        : MainAppletController<Action, Action, BUSAction, UIAction, DataBUSActionFR, BUSUIActionFR>
    {
        public ActionController(ApplicationContext context,
            IWebHostEnvironment environment,
            IScreenInfo screenInfo,
            IViewInfo viewInfo,
            IAppletInfo appletInfo,
            IAppletInfoUI appletInfoUI)
            : base(context,
                  environment,
                  context.Actions,
                  context.Actions,
                  screenInfo,
                  viewInfo,
                  appletInfo,
                  appletInfoUI)
        { }
    }
}