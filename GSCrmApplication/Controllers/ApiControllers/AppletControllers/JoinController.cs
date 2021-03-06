﻿using Microsoft.AspNetCore.Mvc;
using GSCrmApplication.Data;
using GSCrmLibrary.Services.Info;
using GSCrmApplication.Factories.MainFactories;

namespace GSCrmApplication.Controllers.ApiControllers.AppletControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JoinController : GSCrmLibrary.Controllers.ApiControllers.AppletControllers.JoinController<GSAppContext, BUSFactory>
    {
        public JoinController(GSAppContext context,
            IScreenInfo screenInfo,
            IViewInfo viewInfo)
            : base(context, screenInfo, viewInfo)
        { }
    }
}