using Microsoft.AspNetCore.Mvc;

namespace GSCrmTools.Controllers.MVCControllers
{
    public class TableScreenController : Controller
    {
        [Route("Table Screen")]
        public IActionResult Index() => View();
    }
}
