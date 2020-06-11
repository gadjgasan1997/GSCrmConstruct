using Microsoft.AspNetCore.Mvc;
using GSCrmApplication.Data;
using GSCrmLibrary.Services.Info;
using GSCrmApplication.Factories.MainFactories;

namespace GSCrmApplication.Controllers.ApiControllers.BusinessComponentControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JoinController : GSCrmLibrary.Controllers.ApiControllers.BusinessComponentControllers.JoinController<GSAppContext, BUSFactory>
    {
        public JoinController(GSAppContext context, IViewInfo viewInfo)
            : base(context, viewInfo)
        { }
    }
}