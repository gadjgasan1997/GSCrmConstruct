using System.Linq;
using Microsoft.AspNetCore.Mvc;
using GSCrmApplication.Data;
using GSCrmApplication.Models.TableModels;
using GSCrmApplication.Models.AppletModels;
using GSCrmApplication.Models.BusinessComponentModels;
using GSCrmLibrary.Services.Info;
using GSCrmApplication.Factories.DataBUSFactories;
using GSCrmLibrary.Controllers.ApiControllers.MainControllers;
using GSCrmApplication.Factories.MainFactories;
using SysPosition_1 = GSCrmApplication.Models.TableModels.Position;
using SysPosition_2 = GSCrmApplication.Models.BusinessComponentModels.Position;

namespace GSCrmApplication.Controllers.ApiControllers.BusinessComponentControllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PositionController : MainBusinessComponentController
        <SysPosition_1, SysPosition_2, GSAppContext, DataBUSPositionFR, BUSFactory>
	{
        public PositionController(GSAppContext context, IViewInfo viewInfo)
            : base(context, viewInfo)
        {}
	}
}