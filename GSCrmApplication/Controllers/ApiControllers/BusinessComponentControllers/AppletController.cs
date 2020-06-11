using Microsoft.AspNetCore.Mvc;
using GSCrmApplication.Data;
using GSCrmLibrary.Services.Info;
using GSCrmApplication.Factories.MainFactories;

namespace GSCrmApplication.Controllers.ApiControllers.BusinessComponentControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppletController : GSCrmLibrary.Controllers.ApiControllers.BusinessComponentControllers.AppletController<GSAppContext, BUSFactory>
    {
        public AppletController(GSAppContext context, IViewInfo viewInfo)
            : base (context, viewInfo) 
        { }
    }
}