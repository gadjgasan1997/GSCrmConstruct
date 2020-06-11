using Microsoft.AspNetCore.Mvc;
using GSCrmApplication.Data;
using GSCrmLibrary.Services.Info;
using GSCrmApplication.Factories.MainFactories;

namespace GSCrmApplication.Controllers.ApiControllers.BusinessComponentControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControlUPController : GSCrmLibrary.Controllers.ApiControllers.BusinessComponentControllers.ControlUPController<GSAppContext, BUSFactory>
    {
        public ControlUPController(GSAppContext context, IViewInfo viewInfo)
            : base(context, viewInfo)
        { }
    }
}