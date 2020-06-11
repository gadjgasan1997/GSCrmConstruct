using Microsoft.AspNetCore.Mvc;

namespace GSCrmTools.Controllers.MVCControllers
{
    public class DirectoriesListScreenController : Controller
    {
        [Route("Directories List Screen")]
        public IActionResult Index() => View();
    }
}
