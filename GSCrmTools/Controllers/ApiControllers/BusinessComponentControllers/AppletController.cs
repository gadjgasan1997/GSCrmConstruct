﻿using Microsoft.AspNetCore.Mvc;
using GSCrmTools.Data;
using GSCrmLibrary.Services.Info;
using GSCrmTools.Factories.MainFactories;

namespace GSCrmTools.Controllers.ApiControllers.BusinessComponentControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppletController : GSCrmLibrary.Controllers.ApiControllers.BusinessComponentControllers.AppletController<ToolsContext, BUSFactory>
    {
        public AppletController(ToolsContext context, IViewInfo viewInfo)
            : base (context, viewInfo) 
        { }
    }
}