﻿using System.Linq;
using Microsoft.AspNetCore.Mvc;
using GSCrmLibrary.Data;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Models.AppletModels;
using GSCrmLibrary.Models.BusinessComponentModels;
using GSCrmLibrary.Services.Info;
using GSCrmLibrary.Factories.BUSUIFactories;
using GSCrmLibrary.Factories.DataBUSFactories;
using GSCrmLibrary.Controllers.ApiControllers.MainControllers;
using GSCrmLibrary.Factories.MainFactories;
using GSCrmLibrary.Controllers.ApiControllers.BusinessComponentControllers;

namespace GSCrmLibrary.Controllers.ApiControllers.AppletControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControlController<TContext, TBUSFactory>
        : MainAppletController<Control, BUSControl, UIControl, TContext, DataBUSControlFR<TContext>, TBUSFactory, BUSUIControlFR<TContext>>
        where TContext : MainContext, new()
        where TBUSFactory : BUSFactory<TContext>, new()
    {
        public ControlController(TContext context, IScreenInfo screenInfo, IViewInfo viewInfo)
            : base(context, screenInfo, viewInfo)
        { }
    }
}