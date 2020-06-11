using Microsoft.AspNetCore.Mvc;
using GSCrmApplication.Data;
using GSCrmLibrary.Services.Info;
using GSCrmApplication.Factories.MainFactories;

namespace GSCrmApplication.Controllers.ApiControllers.BusinessComponentControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhysicalRenderController : GSCrmLibrary.Controllers.ApiControllers.BusinessComponentControllers.PhysicalRenderController<GSAppContext, BUSFactory>
    {
        public PhysicalRenderController(GSAppContext context, IViewInfo viewInfo)
            : base(context, viewInfo)
        { }
    }
}
