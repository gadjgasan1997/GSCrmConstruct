using Microsoft.AspNetCore.Mvc;
using GSCrmApplication.Data;
using GSCrmLibrary.Services.Info;
using GSCrmApplication.Factories.MainFactories;

namespace GSCrmApplication.Controllers.ApiControllers.BusinessComponentControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PickListController : GSCrmLibrary.Controllers.ApiControllers.BusinessComponentControllers.PickListController<GSAppContext, BUSFactory>
    {
        public PickListController(GSAppContext context, IViewInfo viewInfo)
            : base(context, viewInfo) 
        { }
    }
}
