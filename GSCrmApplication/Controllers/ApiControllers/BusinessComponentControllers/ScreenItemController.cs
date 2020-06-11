using Microsoft.AspNetCore.Mvc;
using GSCrmApplication.Data;
using GSCrmLibrary.Services.Info;
using GSCrmApplication.Factories.MainFactories;

namespace GSCrmApplication.Controllers.ApiControllers.BusinessComponentControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScreenItemController : GSCrmLibrary.Controllers.ApiControllers.BusinessComponentControllers.ScreenItemController<GSAppContext, BUSFactory>
    {
        public ScreenItemController(GSAppContext context, IViewInfo viewInfo)
            : base (context, viewInfo)
        { }
    }
}
