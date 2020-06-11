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
using SysCreate_Employee_Popup_Applet = GSCrmApplication.Models.AppletModels.Create_Employee_Popup_Applet;

namespace GSCrmApplication.Factories.BUSUIFactories
{
	public class BUSUICreate_Employee_Popup_AppletFR : BUSUIFactory<SysEmployee, SysCreate_Employee_Popup_Applet, GSAppContext>
	{
        public override SysEmployee UIToBusiness(SysCreate_Employee_Popup_Applet UIEntity, GSAppContext context, IViewInfo viewInfo, bool NewRecord)
        {
            SysEmployee businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, NewRecord);
            businessEntity.MiddleName = UIEntity.MiddleName;
            businessEntity.ManagerFullName = UIEntity.ManagerFullName;
            businessEntity.LastName = UIEntity.LastName;
            businessEntity.FirstName = UIEntity.FirstName;
            return businessEntity;
        }

        public override SysCreate_Employee_Popup_Applet BusinessToUI(SysEmployee businessEntity)
        {
            SysCreate_Employee_Popup_Applet UIEntity = base.BusinessToUI(businessEntity);
            UIEntity.MiddleName = businessEntity.MiddleName;
            UIEntity.ManagerFullName = businessEntity.ManagerFullName;
            UIEntity.LastName = businessEntity.LastName;
            UIEntity.FirstName = businessEntity.FirstName;
            return UIEntity;
        }

        public override IEnumerable<ValidationResult> UIValidate(GSAppContext context, IViewInfo viewInfo, SysCreate_Employee_Popup_Applet UIEntity, bool isNewRecord)
        {
            List<ValidationResult> result = base.UIValidate(context, viewInfo, UIEntity, isNewRecord).ToList();
            return result;
        }

        public override IEnumerable<ValidationResult> BUSUIValidate(GSAppContext context, SysEmployee businessEntity, SysCreate_Employee_Popup_Applet UIEntity)
        {
            List<ValidationResult> result = base.BUSUIValidate(context, businessEntity, UIEntity).ToList();
            return result;
        }
	}
}
