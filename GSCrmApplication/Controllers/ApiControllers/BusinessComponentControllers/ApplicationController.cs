using Microsoft.AspNetCore.Mvc;
using GSCrmApplication.Data;
using GSCrmLibrary.Services.Info;
using GSCrmApplication.Factories.MainFactories;

namespace GSCrmApplication.Controllers.ApiControllers.BusinessComponentControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : GSCrmLibrary.Controllers.ApiControllers.BusinessComponentControllers.ApplicationController<GSAppContext, BUSFactory>
    {
        public ApplicationController(GSAppContext context, IViewInfo viewInfo)
            : base(context, viewInfo)
        { }
    }
}