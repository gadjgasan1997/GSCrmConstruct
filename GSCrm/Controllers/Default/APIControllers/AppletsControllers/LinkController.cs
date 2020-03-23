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
    public class LinkController
        : MainAppletController<Link, Link, BUSLink, UILink, DataBUSLinkFR, BUSUILinkFR>
    {
        public LinkController(ApplicationContext context,
            IWebHostEnvironment environment,
            IScreenInfo screenInfo,
            IViewInfo viewInfo,
            IAppletInfo appletInfo,
            IAppletInfoUI appletInfoUI)
            : base(context,
                  environment,
                  context.Links,
                  context.Links,
                  screenInfo,
                  viewInfo,
                  appletInfo,
                  appletInfoUI) 
        { }
    }
}