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
using SysEmployee_Form_Applet = GSCrmApplication.Models.AppletModels.Employee_Form_Applet;

namespace GSCrmApplication.Factories.BUSUIFactories
{
	public class BUSUIEmployee_Form_AppletFR : BUSUIFactory<SysEmployee, SysEmployee_Form_Applet, GSAppContext>
	{
        public override SysEmployee UIToBusiness(SysEmployee_Form_Applet UIEntity, GSAppContext context, IViewInfo viewInfo, bool NewRecord)
        {
            SysEmployee businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, NewRecord);
            businessEntity.MiddleName = UIEntity.MiddleName;
            return businessEntity;
        }

        public override SysEmployee_Form_Applet BusinessToUI(SysEmployee businessEntity)
        {
            SysEmployee_Form_Applet UIEntity = base.BusinessToUI(businessEntity);
            UIEntity.MiddleName = businessEntity.MiddleName;
            return UIEntity;
        }

        public override IEnumerable<ValidationResult> UIValidate(GSAppContext context, IViewInfo viewInfo, SysEmployee_Form_Applet UIEntity, bool isNewRecord)
        {
            List<ValidationResult> result = base.UIValidate(context, viewInfo, UIEntity, isNewRecord).ToList();
            return result;
        }

        public override IEnumerable<ValidationResult> BUSUIValidate(GSAppContext context, SysEmployee businessEntity, SysEmployee_Form_Applet UIEntity)
        {
            List<ValidationResult> result = base.BUSUIValidate(context, businessEntity, UIEntity).ToList();
            return result;
        }
	}
}
