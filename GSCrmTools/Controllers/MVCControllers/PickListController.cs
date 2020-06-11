using Microsoft.AspNetCore.Mvc;

namespace GSCrmTools.Controllers.MVCControllers
{
    public class PickListScreenController : Controller
    {
        [Route("PickList Screen")]
        public IActionResult Index() => View();
    }
}
