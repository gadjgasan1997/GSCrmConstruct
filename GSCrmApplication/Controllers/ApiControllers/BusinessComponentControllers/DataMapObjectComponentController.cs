using Microsoft.AspNetCore.Mvc;
using GSCrmApplication.Data;
using GSCrmLibrary.Services.Info;
using GSCrmApplication.Factories.MainFactories;

namespace GSCrmApplication.Controllers.ApiControllers.BusinessComponentControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataMapObjectComponentController : GSCrmLibrary.Controllers.ApiControllers.BusinessComponentControllers.DataMapObjectComponentController<GSAppContext, BUSFactory>
    {
        public DataMapObjectComponentController(GSAppContext context, IViewInfo viewInfo)
            : base(context, viewInfo)
        { }
    }
}