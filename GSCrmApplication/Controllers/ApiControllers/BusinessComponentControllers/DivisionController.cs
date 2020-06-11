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
using SysDivision_1 = GSCrmApplication.Models.TableModels.Division;
using SysDivision_2 = GSCrmApplication.Models.BusinessComponentModels.Division;

namespace GSCrmApplication.Controllers.ApiControllers.BusinessComponentControllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DivisionController : MainBusinessComponentController
        <SysDivision_1, SysDivision_2, GSAppContext, DataBUSDivisionFR, BUSFactory>
	{
        public DivisionController(GSAppContext context, IViewInfo viewInfo)
            : base(context, viewInfo)
        {}
	}
}