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
using SysEmployee_1 = GSCrmApplication.Models.TableModels.Employee;
using SysEmployee_2 = GSCrmApplication.Models.BusinessComponentModels.Employee;

namespace GSCrmApplication.Controllers.ApiControllers.BusinessComponentControllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class EmployeeController : MainBusinessComponentController
        <SysEmployee_1, SysEmployee_2, GSAppContext, DataBUSEmployeeFR, BUSFactory>
	{
        public EmployeeController(GSAppContext context, IViewInfo viewInfo)
            : base(context, viewInfo)
        {}
	}
}