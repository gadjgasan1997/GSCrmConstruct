using Microsoft.AspNetCore.Mvc;
using GSCrmApplication.Data;
using GSCrmLibrary.Services.Info;
using GSCrmApplication.Factories.MainFactories;

namespace GSCrmApplication.Controllers.ApiControllers.BusinessComponentControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TableController : GSCrmLibrary.Controllers.ApiControllers.BusinessComponentControllers.TableController<GSAppContext, BUSFactory>
    {
        public TableController(GSAppContext context, IViewInfo viewInfo)
            : base (context, viewInfo)
        { }
    }
}
