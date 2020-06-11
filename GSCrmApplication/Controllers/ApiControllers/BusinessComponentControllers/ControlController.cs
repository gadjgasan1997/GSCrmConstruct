using Microsoft.AspNetCore.Mvc;
using GSCrmApplication.Data;
using GSCrmLibrary.Services.Info;
using GSCrmApplication.Factories.MainFactories;

namespace GSCrmApplication.Controllers.ApiControllers.BusinessComponentControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControlController : GSCrmLibrary.Controllers.ApiControllers.BusinessComponentControllers.ControlController<GSAppContext, BUSFactory>
    {
        public ControlController(GSAppContext context, IViewInfo viewInfo)
            : base(context, viewInfo)
        { }
    }
}