﻿using Microsoft.AspNetCore.Mvc;
using GSCrm.Controllers.Default.APIControllers.MainControllers;
using GSCrm.Data;
using GSCrm.Services.Info;

namespace GSCrm.Controllers.Default.APIControllers.ScreenControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViewsController : MainScreenController
    {
        public ViewsController(ApplicationContext context,
            IScreenInfo screenInfo,
            IScreenInfoUI screenInfoUI,
            IViewInfo viewInfo)
            : base (context,
                  screenInfo,
                  screenInfoUI,
                  viewInfo)
        { }
    }
}