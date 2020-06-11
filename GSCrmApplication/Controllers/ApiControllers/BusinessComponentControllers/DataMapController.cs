using Microsoft.AspNetCore.Mvc;
using GSCrmApplication.Data;
using GSCrmLibrary.Services.Info;
using GSCrmApplication.Factories.MainFactories;

namespace GSCrmApplication.Controllers.ApiControllers.BusinessComponentControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataMapController : GSCrmLibrary.Controllers.ApiControllers.BusinessComponentControllers.DataMapController<GSAppContext, BUSFactory>
    {
        public DataMapController(GSAppContext context, IViewInfo viewInfo)
            : base(context, viewInfo)
        { }
    }
}