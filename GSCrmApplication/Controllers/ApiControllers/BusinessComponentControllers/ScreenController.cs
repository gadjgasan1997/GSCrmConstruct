using Microsoft.AspNetCore.Mvc;
using GSCrmApplication.Data;
using GSCrmLibrary.Services.Info;
using GSCrmApplication.Factories.MainFactories;

namespace GSCrmApplication.Controllers.ApiControllers.BusinessComponentControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScreenController : GSCrmLibrary.Controllers.ApiControllers.BusinessComponentControllers.ScreenController<GSAppContext, BUSFactory>
    {
        public ScreenController(GSAppContext context, IViewInfo viewInfo)
            : base (context, viewInfo)
        { }
    }
}
