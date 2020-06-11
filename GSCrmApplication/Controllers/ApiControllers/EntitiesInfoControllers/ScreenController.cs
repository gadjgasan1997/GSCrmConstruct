using Microsoft.AspNetCore.Mvc;
using GSCrmLibrary.Services.Info;
using GSCrmApplication.Data;

namespace GSCrmApplication.Controllers.ApiControllers.EntitiesInfoControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScreenController : GSCrmLibrary.Controllers.ApiControllers.EntitiesInfoControllers.ScreenController<GSAppContext>
    {
        public ScreenController(GSAppContext context,
            IScreenInfo screenInfo,
            IScreenInfoUI screenInfoUI,
            IViewInfo viewInfo)
            : base(context, screenInfo, screenInfoUI, viewInfo)
        { }
    }
}
