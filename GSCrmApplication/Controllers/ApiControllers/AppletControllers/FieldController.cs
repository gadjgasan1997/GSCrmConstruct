﻿using Microsoft.AspNetCore.Mvc;
using GSCrmApplication.Data;
using GSCrmLibrary.Services.Info;
using GSCrmApplication.Factories.MainFactories;

namespace GSCrmApplication.Controllers.ApiControllers.AppletControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FieldController : GSCrmLibrary.Controllers.ApiControllers.AppletControllers.FieldController<GSAppContext, BUSFactory>
    {
        public FieldController(GSAppContext context,
            IScreenInfo screenInfo,
            IViewInfo viewInfo)
            : base(context, screenInfo, viewInfo) 
        { }
    }
}