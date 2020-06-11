using Microsoft.AspNetCore.Mvc;
using GSCrmApplication.Data;
using GSCrmLibrary.Services.Info;
using GSCrmApplication.Factories.MainFactories;

namespace GSCrmApplication.Controllers.ApiControllers.BusinessComponentControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataMapFieldController : GSCrmLibrary.Controllers.ApiControllers.BusinessComponentControllers.DataMapFieldController<GSAppContext, BUSFactory>
    {
        public DataMapFieldController(GSAppContext context, IViewInfo viewInfo)
            : base(context, viewInfo)
        { }
    }
}