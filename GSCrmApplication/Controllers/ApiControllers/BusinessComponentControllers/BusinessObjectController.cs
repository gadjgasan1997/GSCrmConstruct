using Microsoft.AspNetCore.Mvc;
using GSCrmApplication.Data;
using GSCrmLibrary.Services.Info;
using GSCrmApplication.Factories.MainFactories;

namespace GSCrmApplication.Controllers.ApiControllers.BusinessComponentControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessObjectController : GSCrmLibrary.Controllers.ApiControllers.BusinessComponentControllers.BusinessObjectController<GSAppContext, BUSFactory>
    {
        public BusinessObjectController(GSAppContext context, IViewInfo viewInfo)
            : base(context, viewInfo) 
        { }
    }
}