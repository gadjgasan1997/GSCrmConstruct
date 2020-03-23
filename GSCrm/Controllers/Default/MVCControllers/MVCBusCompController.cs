using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GSCrm.Models;

namespace GSCrm.Controllers.Default.MVCControllers
{
    public class MVCBusCompController : Controller
    {
        [Route("DevTools/BusinessComponents")]
        public IActionResult BusinessComponents()
        {
            return View();
        }
    }
}
