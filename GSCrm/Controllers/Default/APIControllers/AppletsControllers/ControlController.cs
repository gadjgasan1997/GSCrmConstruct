using GSCrm.Data;
using GSCrm.Models.Default.TableModels;
using GSCrm.Models.Default.AppletModels;
using GSCrm.Models.Default.BusinessComponentModels;
using GSCrm.Services.Info;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using GSCrm.Factories.Default.BUSUIFactories;
using GSCrm.Factories.Default.DataBUSFactories;
using GSCrm.Controllers.Default.APIControllers.MainControllers;

namespace GSCrm.Controllers.Default.APIControllers.AppletsControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControlController
        : MainAppletController<Applet, Control, BUSControl, UIControl, DataBUSControlFR, BUSUIControlFR>
    {
        public ControlController(ApplicationContext context,
            IWebHostEnvironment environment,
            IScreenInfo screenInfo,
            IViewInfo viewInfo,
            IAppletInfo appletInfo,
            IAppletInfoUI appletInfoUI)
            : base(context,
                  environment,
                  context.Applets,
                  context.Controls,
                  screenInfo,
                  viewInfo,
                  appletInfo,
                  appletInfoUI)
        { }
    }
}