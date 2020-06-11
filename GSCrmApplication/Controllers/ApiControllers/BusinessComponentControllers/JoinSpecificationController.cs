using Microsoft.AspNetCore.Mvc;
using GSCrmApplication.Data;
using GSCrmLibrary.Services.Info;
using GSCrmApplication.Factories.MainFactories;

namespace GSCrmApplication.Controllers.ApiControllers.BusinessComponentControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JoinSpecificationController : GSCrmLibrary.Controllers.ApiControllers.BusinessComponentControllers.JoinSpecificationController<GSAppContext, BUSFactory>
    {
        public JoinSpecificationController(GSAppContext context, IViewInfo viewInfo)
            : base(context, viewInfo)
        { }
    }
}