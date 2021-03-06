﻿using Microsoft.AspNetCore.Mvc;
using GSCrmApplication.Data;
using GSCrmLibrary.Services.Info;
using GSCrmApplication.Factories.MainFactories;

namespace GSCrmApplication.Controllers.ApiControllers.AppletControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataMapObjectController : GSCrmLibrary.Controllers.ApiControllers.AppletControllers.DataMapObjectController<GSAppContext, BUSFactory>
    {
        public DataMapObjectController(GSAppContext context, IScreenInfo screenInfo, IViewInfo viewInfo)
            : base(context, screenInfo, viewInfo)
        { }
    }
}