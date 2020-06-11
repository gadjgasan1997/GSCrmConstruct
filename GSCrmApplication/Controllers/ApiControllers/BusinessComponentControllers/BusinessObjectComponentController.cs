using GSCrmApplication.Data;
using Microsoft.AspNetCore.Mvc;
using GSCrmLibrary.Services.Info;
using GSCrmApplication.Factories.MainFactories;

namespace GSCrmApplication.Controllers.ApiControllers.BusinessComponentControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessObjectComponentController : GSCrmLibrary.Controllers.ApiControllers.BusinessComponentControllers.BusinessObjectComponentController<GSAppContext, BUSFactory>
    {
        public BusinessObjectComponentController(GSAppContext context, IViewInfo viewInfo)
            : base(context, viewInfo) 
        { }
    }
}