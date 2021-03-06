﻿using Microsoft.AspNetCore.Mvc;
using GSCrmTools.Data;
using GSCrmLibrary.Services.Info;
using GSCrmTools.Factories.MainFactories;

namespace GSCrmTools.Controllers.ApiControllers.AppletControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : GSCrmLibrary.Controllers.ApiControllers.AppletControllers.ApplicationController<ToolsContext, BUSFactory>
    {
        public ApplicationController(ToolsContext context,
            IScreenInfo screenInfo,
            IViewInfo viewInfo)
            : base(context, screenInfo, viewInfo)
        { }
    }
}