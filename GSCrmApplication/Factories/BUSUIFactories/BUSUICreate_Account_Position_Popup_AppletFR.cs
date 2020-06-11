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
using SysPosition = GSCrmApplication.Models.BusinessComponentModels.Position;
using SysCreate_Account_Position_Popup_Applet = GSCrmApplication.Models.AppletModels.Create_Account_Position_Popup_Applet;

namespace GSCrmApplication.Factories.BUSUIFactories
{
	public class BUSUICreate_Account_Position_Popup_AppletFR : BUSUIFactory<SysPosition, SysCreate_Account_Position_Popup_Applet, GSAppContext>
	{
        public override SysPosition UIToBusiness(SysCreate_Account_Position_Popup_Applet UIEntity, GSAppContext context, IViewInfo viewInfo, bool NewRecord)
        {
            SysPosition businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, NewRecord);
            businessEntity.AccountId = Guid.Parse(ComponentsRecordsInfo.GetSelectedRecord("Account"));
            businessEntity.ParentPositionName = UIEntity.ParentPositionName;
            businessEntity.Name = UIEntity.Name;
            businessEntity.PrimaryEmployeeFullName = UIEntity.PrimaryEmployeeFullName;
            return businessEntity;
        }

        public override SysCreate_Account_Position_Popup_Applet BusinessToUI(SysPosition businessEntity)
        {
            SysCreate_Account_Position_Popup_Applet UIEntity = base.BusinessToUI(businessEntity);
            UIEntity.ParentPositionName = businessEntity.ParentPositionName;
            UIEntity.Name = businessEntity.Name;
            UIEntity.PrimaryEmployeeFullName = businessEntity.PrimaryEmployeeFullName;
            return UIEntity;
        }

        public override IEnumerable<ValidationResult> UIValidate(GSAppContext context, IViewInfo viewInfo, SysCreate_Account_Position_Popup_Applet UIEntity, bool isNewRecord)
        {
            List<ValidationResult> result = base.UIValidate(context, viewInfo, UIEntity, isNewRecord).ToList();
            return result;
        }

        public override IEnumerable<ValidationResult> BUSUIValidate(GSAppContext context, SysPosition businessEntity, SysCreate_Account_Position_Popup_Applet UIEntity)
        {
            List<ValidationResult> result = base.BUSUIValidate(context, businessEntity, UIEntity).ToList();
            return result;
        }
	}
}
