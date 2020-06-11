using Microsoft.AspNetCore.Mvc;
using GSCrmLibrary.Services.Info;
using GSCrmApplication.Data;

namespace GSCrmApplication.Controllers.ApiControllers.EntitiesInfoControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppletController : GSCrmLibrary.Controllers.ApiControllers.EntitiesInfoControllers.AppletController<GSAppContext>
    {
        public AppletController(GSAppContext context,
            IAppletInfo appletInfo,
            IAppletInfoUI appletInfoUI)
            : base(context, appletInfo, appletInfoUI)
        { }
    }
}