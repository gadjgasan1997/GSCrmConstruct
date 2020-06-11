using Microsoft.AspNetCore.Mvc;

namespace GSCrmTools.Controllers.MVCControllers
{
    public class ViewScreenController : Controller
    {
        [Route("View Screen")]
        public IActionResult Index() => View();
    }
}
