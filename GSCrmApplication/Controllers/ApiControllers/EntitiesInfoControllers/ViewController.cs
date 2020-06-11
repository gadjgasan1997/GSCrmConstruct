using Microsoft.AspNetCore.Mvc;
using GSCrmLibrary.Services.Info;
using GSCrmApplication.Data;
using GSCrmApplication.Factories.MainFactories;

namespace GSCrmApplication.Controllers.ApiControllers.EntitiesInfoControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViewController : GSCrmLibrary.Controllers.ApiControllers.EntitiesInfoControllers.ViewController<GSAppContext, BUSFactory>
    {
        public ViewController(GSAppContext context,
            IViewInfo viewInfo,
            IViewInfoUI viewInfoUI)
            : base(context, viewInfo, viewInfoUI)
        { }
    }
}
