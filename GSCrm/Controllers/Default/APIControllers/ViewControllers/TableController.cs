using Microsoft.AspNetCore.Mvc;
using GSCrm.Controllers.Default.APIControllers.MainControllers;
using GSCrm.Data;
using GSCrm.Services.Info;

namespace GSCrm.Controllers.Default.APIControllers.ViewControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TableController : MainViewController
    {
        public TableController(ApplicationContext context,
            IViewInfo viewInfo,
            IViewInfoUI viewInfoUI)
            : base(context,
                  viewInfo,
                  viewInfoUI)
        { }
    }
}
