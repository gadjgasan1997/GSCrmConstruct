using GSCrm.Data;
using GSCrm.Models.Default.TableModels;
using GSCrm.Models.Default.AppletModels;
using GSCrm.Models.Default.BusinessComponentModels;
using Microsoft.AspNetCore.Mvc;
using GSCrm.Services.Info;
using Microsoft.AspNetCore.Hosting;
using GSCrm.Factories.Default.BUSUIFactories;
using GSCrm.Factories.Default.DataBUSFactories;
using GSCrm.Controllers.Default.APIControllers.MainControllers;

namespace GSCrm.Controllers.Default.APIControllers.AppletsControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BOComponentController
        : MainAppletController<BusObject, BOComponent, BUSBOComponent, UIBOComponent, DataBUSBOComponentFR, BUSUIBOComponentFR>
    {
        public BOComponentController(ApplicationContext context,
            IWebHostEnvironment environment,
            IScreenInfo screenInfo,
            IViewInfo viewInfo,
            IAppletInfo appletInfo,
            IAppletInfoUI appletInfoUI)
            : base(context,
                  environment,
                  context.BusinessObjects,
                  context.BusinessObjectComponents,
                  screenInfo,
                  viewInfo,
                  appletInfo,
                  appletInfoUI) 
        { }
    }
}