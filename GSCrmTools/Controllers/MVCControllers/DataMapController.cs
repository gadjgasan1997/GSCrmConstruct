using Microsoft.AspNetCore.Mvc;

namespace GSCrmTools.Controllers.MVCControllers
{
    public class DataMapScreenController : Controller
    {
        [Route("Data Map Screen")]
        public IActionResult Index() => View();
    }
}
