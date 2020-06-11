using Microsoft.AspNetCore.Mvc;
using GSCrmApplication.Data;
using GSCrmLibrary.Services.Info;
using GSCrmApplication.Factories.MainFactories;

namespace GSCrmApplication.Controllers.ApiControllers.BusinessComponentControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViewItemController : GSCrmLibrary.Controllers.ApiControllers.BusinessComponentControllers.ViewItemController<GSAppContext, BUSFactory>
    {
        public ViewItemController(GSAppContext context, IViewInfo viewInfo)
            : base(context, viewInfo) 
        { }
    }
}