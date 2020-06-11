using Microsoft.AspNetCore.Mvc;
using GSCrmApplication.Data;
using GSCrmLibrary.Services.Info;
using GSCrmApplication.Factories.MainFactories;

namespace GSCrmApplication.Controllers.ApiControllers.BusinessComponentControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColumnController : GSCrmLibrary.Controllers.ApiControllers.BusinessComponentControllers.ColumnController<GSAppContext, BUSFactory>
    {
        public ColumnController(GSAppContext context, IViewInfo viewInfo)
            : base(context, viewInfo) 
        { }

    }
}
