using Microsoft.AspNetCore.Mvc;
using GSCrmLibrary.Services.Info;
using GSCrmApplication.Data;
using GSCrmApplication.Factories.MainFactories;

namespace GSCrmApplication.Controllers.ApiControllers.EntitiesInfoControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : GSCrmLibrary.Controllers.ApiControllers.EntitiesInfoControllers.ApplicationController<GSAppContext, BUSFactory>
    {
        public ApplicationController(GSAppContext context,
            IApplicationInfo applicationInfo,
            IApplicationInfoUI applicationInfoUI,
            IScreenInfo screenInfo,
            IScreenInfoUI screenInfoUI,
            IViewInfo viewInfo)
            : base(context, applicationInfo, applicationInfoUI, screenInfo, screenInfoUI, viewInfo)
        { }
    }
}
