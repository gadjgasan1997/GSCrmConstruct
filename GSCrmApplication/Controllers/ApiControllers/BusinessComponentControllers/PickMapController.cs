using Microsoft.AspNetCore.Mvc;
using GSCrmApplication.Data;
using GSCrmLibrary.Services.Info;
using GSCrmApplication.Factories.MainFactories;

namespace GSCrmApplication.Controllers.ApiControllers.BusinessComponentControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PickMapController : GSCrmLibrary.Controllers.ApiControllers.BusinessComponentControllers.PickMapController<GSAppContext, BUSFactory>
    {
        public PickMapController(GSAppContext context, IViewInfo viewInfo)
            : base(context, viewInfo)
        { }
    }
}