using GSCrmApplication.Data;
using Microsoft.AspNetCore.Mvc;
using GSCrmLibrary.Services.Info;
using GSCrmApplication.Factories.MainFactories;

namespace GSCrmApplication.Controllers.ApiControllers.AppletControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessObjectComponentController : GSCrmLibrary.Controllers.ApiControllers.AppletControllers.BusinessObjectComponentController<GSAppContext, BUSFactory>
    {
        public BusinessObjectComponentController(GSAppContext context,
            IScreenInfo screenInfo,
            IViewInfo viewInfo)
            : base(context, screenInfo, viewInfo) 
        { }
    }
}