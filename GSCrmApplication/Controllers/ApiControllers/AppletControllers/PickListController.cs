using Microsoft.AspNetCore.Mvc;
using GSCrmApplication.Data;
using GSCrmLibrary.Services.Info;
using GSCrmApplication.Factories.MainFactories;

namespace GSCrmApplication.Controllers.ApiControllers.AppletControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PickListController : GSCrmLibrary.Controllers.ApiControllers.AppletControllers.PickListController<GSAppContext, BUSFactory>
    {
        public PickListController(GSAppContext context,
            IScreenInfo screenInfo,
            IViewInfo viewInfo)
            : base(context, screenInfo, viewInfo) 
        { }
    }
}
