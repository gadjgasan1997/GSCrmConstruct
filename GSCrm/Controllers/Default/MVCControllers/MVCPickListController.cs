using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GSCrm.Models;
using System.ComponentModel.DataAnnotations;

namespace GSCrm.Controllers.Default.MVCControllers
{
    public class MVCPickListController : Controller
    {
        [Route("DevTools/PickLists")]
        public IActionResult PickLists()
        {
            return View();
        }
    }
}
