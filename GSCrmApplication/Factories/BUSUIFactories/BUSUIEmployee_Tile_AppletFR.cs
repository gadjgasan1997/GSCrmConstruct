using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using GSCrmApplication.Data;
using GSCrmLibrary.Models.MainEntities;
using GSCrmLibrary.Models.AppletModels;
using GSCrmLibrary.Models.TableModels;
using GSCrmApplication.Factories.MainFactories;
using GSCrmApplication.Models.TableModels;
using GSCrmLibrary.Services.Info;
using GSCrmLibrary.Data;
using SysEmployee = GSCrmApplication.Models.BusinessComponentModels.Employee;
using SysEmployee_Tile_Applet = GSCrmApplication.Models.AppletModels.Employee_Tile_Applet;

namespace GSCrmApplication.Factories.BUSUIFactories
{
	public class BUSUIEmployee_Tile_AppletFR : BUSUIFactory<SysEmployee, SysEmployee_Tile_Applet, GSAppContext>
	{
        public override SysEmployee UIToBusiness(SysEmployee_Tile_Applet UIEntity, GSAppContext context, IViewInfo viewInfo, bool NewRecord)
        {
            SysEmployee businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, NewRecord);
            return businessEntity;
        }

        public override SysEmployee_Tile_Applet BusinessToUI(SysEmployee businessEntity)
        {
            SysEmployee_Tile_Applet UIEntity = base.BusinessToUI(businessEntity);
            return UIEntity;
        }

        public override IEnumerable<ValidationResult> UIValidate(GSAppContext context, IViewInfo viewInfo, SysEmployee_Tile_Applet UIEntity, bool isNewRecord)
        {
            List<ValidationResult> result = base.UIValidate(context, viewInfo, UIEntity, isNewRecord).ToList();
            return result;
        }

        public override IEnumerable<ValidationResult> BUSUIValidate(GSAppContext context, SysEmployee businessEntity, SysEmployee_Tile_Applet UIEntity)
        {
            List<ValidationResult> result = base.BUSUIValidate(context, businessEntity, UIEntity).ToList();
            return result;
        }
	}
}
