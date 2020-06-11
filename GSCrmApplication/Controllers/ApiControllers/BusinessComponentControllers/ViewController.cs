using Microsoft.AspNetCore.Mvc;
using GSCrmApplication.Data;
using GSCrmLibrary.Services.Info;
using GSCrmApplication.Factories.MainFactories;

namespace GSCrmApplication.Controllers.ApiControllers.BusinessComponentControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViewController : GSCrmLibrary.Controllers.ApiControllers.BusinessComponentControllers.ViewController<GSAppContext, BUSFactory>
    {
        public ViewController(GSAppContext context, IViewInfo viewInfo)
            : base(context, viewInfo) 
        { }
    }
}