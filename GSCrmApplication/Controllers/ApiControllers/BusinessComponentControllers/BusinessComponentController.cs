using Microsoft.AspNetCore.Mvc;
using GSCrmApplication.Data;
using GSCrmLibrary.Services.Info;
using GSCrmApplication.Factories.MainFactories;

namespace GSCrmApplication.Controllers.ApiControllers.BusinessComponentControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessComponentController : GSCrmLibrary.Controllers.ApiControllers.BusinessComponentControllers.BusinessComponentController<GSAppContext, BUSFactory>
    {
        public BusinessComponentController(GSAppContext context, IViewInfo viewInfo)
            : base(context, viewInfo)
        { }
    }
}