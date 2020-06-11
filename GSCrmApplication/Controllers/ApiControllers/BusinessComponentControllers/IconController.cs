using Microsoft.AspNetCore.Mvc;
using GSCrmApplication.Data;
using GSCrmLibrary.Services.Info;
using GSCrmApplication.Factories.MainFactories;

namespace GSCrmApplication.Controllers.ApiControllers.BusinessComponentControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IconController : GSCrmLibrary.Controllers.ApiControllers.BusinessComponentControllers.IconController<GSAppContext, BUSFactory>
    {
        public IconController(GSAppContext context, IViewInfo viewInfo)
            : base(context, viewInfo)
        { }
    }
}
