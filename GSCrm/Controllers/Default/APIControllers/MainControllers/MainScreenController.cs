using GSCrm.Data;
using GSCrm.Models.Default.TableModels;
using GSCrm.Models.Default.RequestModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Linq.Dynamic.Core;
using GSCrm.Services.Info;
using GSCrm.Data.Context;

namespace GSCrm.Controllers.Default.APIControllers.MainControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainScreenController : ControllerBase
    {
        private readonly ApplicationContext context;
        private readonly IScreenInfo screenInfo;
        private readonly IScreenInfoUI screenInfoUI;
        private readonly IViewInfo viewInfo;

        public MainScreenController(ApplicationContext context,
            IScreenInfo screenInfo,
            IScreenInfoUI screenInfoUI,
            IViewInfo viewInfo)
        {
            this.context = context;
            this.screenInfo = screenInfo;
            this.screenInfoUI = screenInfoUI;
            this.viewInfo = viewInfo;
        }

        #region Requests
        [HttpGet("InitializeScreen/{screenName}")]
        public virtual object InitializeScreen(string screenName)
        {
            // Проверка, что такой скрин уже не проинициализирован
            if (screenInfo.Screen == null || screenInfo.Screen.Name != screenName)
            {
                // Очистка информации
                viewInfo.Dispose();
                ComponentContext.Dispose();

                // Инициализация скрина
                screenInfo.Initialize(screenName, null, null, context);
                screenInfo.Action = context.Actions.FirstOrDefault(n => n.Name == "InitializeScreen");
                screenInfoUI.Initialize(context);
            }
            else screenInfo.Action = context.Actions.FirstOrDefault(n => n.Name == "ReloadScreen");
            return screenInfoUI.Serialize();
        }

        [HttpGet("ScreenInfo")]
        public virtual object ScreenInfo()
        {
            screenInfoUI.Initialize(context);
            return screenInfoUI.Serialize();
        }
        
        [HttpPost("UpdateScreenInfo")]
        public virtual object UpdateScreenInfo([FromBody] RequestScreenModel model)
        {
            screenInfo.Initialize(model.Name, null, model.ViewName, context);
            screenInfo.Action = context.Actions.FirstOrDefault(n => n.Name == model.Action);
            screenInfoUI.Initialize(context);
            return screenInfoUI.Serialize();
        }
        #endregion
    }
}
