using Microsoft.AspNetCore.Mvc;
using GSCrmApplication.Data;
using GSCrmLibrary.Services.Info;
using GSCrmApplication.Factories.MainFactories;

namespace GSCrmApplication.Controllers.ApiControllers.AppletControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScreenController : GSCrmLibrary.Controllers.ApiControllers.AppletControllers.ScreenController<GSAppContext, BUSFactory>
    {
        public ScreenController(GSAppContext context,
            IScreenInfo screenInfo,
            IViewInfo viewInfo)
            : base (context, screenInfo, viewInfo)
        { }
    }
}
