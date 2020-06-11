using Microsoft.AspNetCore.Mvc;
using GSCrmApplication.Data;
using GSCrmLibrary.Services.Info;
using GSCrmApplication.Factories.MainFactories;

namespace GSCrmApplication.Controllers.ApiControllers.AppletControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationItemController : GSCrmLibrary.Controllers.ApiControllers.AppletControllers.ApplicationItemController<GSAppContext, BUSFactory>
    {
        public ApplicationItemController(GSAppContext context,
            IScreenInfo screenInfo,
            IViewInfo viewInfo)
            : base(context, screenInfo, viewInfo)
        { }
    }
}
