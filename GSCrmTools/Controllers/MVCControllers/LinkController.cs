using Microsoft.AspNetCore.Mvc;

namespace GSCrmTools.Controllers.MVCControllers
{
    public class LinkScreenController : Controller
    {
        [Route("Link Screen")]
        public IActionResult Index() => View();
    }
}
