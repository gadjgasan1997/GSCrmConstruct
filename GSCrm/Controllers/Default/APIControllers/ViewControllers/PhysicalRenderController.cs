using GSCrm.Controllers.Default.APIControllers.MainControllers;
using GSCrm.Data;
using GSCrm.Services.Info;
using Microsoft.AspNetCore.Mvc;

namespace GSCrm.Controllers.Default.APIControllers.ViewControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhysicalRenderController : MainViewController
    {
        public PhysicalRenderController(ApplicationContext context,
            IViewInfo viewInfo,
            IViewInfoUI viewInfoUI)
            : base(context,
                  viewInfo,
                  viewInfoUI)
        { }
    }
}
