using Microsoft.AspNetCore.Mvc;
using GSCrmApplication.Data;
using GSCrmLibrary.Services.Info;
using GSCrmApplication.Factories.MainFactories;

namespace GSCrmApplication.Controllers.ApiControllers.BusinessComponentControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TableColumnController : GSCrmLibrary.Controllers.ApiControllers.BusinessComponentControllers.TableColumnController<GSAppContext, BUSFactory>
    {
        public TableColumnController(GSAppContext context, IViewInfo viewInfo)
            : base(context, viewInfo)
        { }
    }
}
