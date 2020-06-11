using Microsoft.AspNetCore.Mvc;
using GSCrmApplication.Data;
using GSCrmLibrary.Services.Info;
using GSCrmApplication.Factories.MainFactories;

namespace GSCrmApplication.Controllers.ApiControllers.AppletControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppletController : GSCrmLibrary.Controllers.ApiControllers.AppletControllers.AppletController<GSAppContext, BUSFactory>
    {
        public AppletController(GSAppContext context, IScreenInfo screenInfo, IViewInfo viewInfo)
            : base (context, screenInfo, viewInfo) 
        { }
    }
}