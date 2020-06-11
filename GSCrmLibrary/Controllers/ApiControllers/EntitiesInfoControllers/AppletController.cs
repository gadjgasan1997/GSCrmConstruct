using Microsoft.AspNetCore.Mvc;
using GSCrmLibrary.Data;
using GSCrmLibrary.Services.Info;

namespace GSCrmLibrary.Controllers.ApiControllers.EntitiesInfoControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppletController<TContext> : ControllerBase
        where TContext : MainContext, new()
    {
        private readonly TContext context;
        private readonly IAppletInfo appletInfo;
        private readonly IAppletInfoUI appletInfoUI;
        public AppletController(TContext context,
            IAppletInfo appletInfo,
            IAppletInfoUI appletInfoUI)
        {
            this.context = context;
            this.appletInfo = appletInfo;
            this.appletInfoUI = appletInfoUI;
        }

        #region Requests
        [HttpGet("InitializeApplet/{appletName}")]
        public virtual ActionResult<object> AppletInfo(string appletName)
        {
            appletInfo.Initialize(appletName, context);
            appletInfoUI.Initialize(context);
            return Ok(appletInfoUI.Serialize());
        }

        [HttpGet("AppletInfo")]
        public virtual ActionResult<object> AppletInfo() => Ok(appletInfoUI.Serialize());
        #endregion
    }
}