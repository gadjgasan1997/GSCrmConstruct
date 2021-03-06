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
using SysDivision = GSCrmApplication.Models.BusinessComponentModels.Division;
using SysCreate_Account_Division_Popup_Applet = GSCrmApplication.Models.AppletModels.Create_Account_Division_Popup_Applet;

namespace GSCrmApplication.Factories.BUSUIFactories
{
	public class BUSUICreate_Account_Division_Popup_AppletFR : BUSUIFactory<SysDivision, SysCreate_Account_Division_Popup_Applet, GSAppContext>
	{
        public override SysDivision UIToBusiness(SysCreate_Account_Division_Popup_Applet UIEntity, GSAppContext context, IViewInfo viewInfo, bool NewRecord)
        {
            SysDivision businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, NewRecord);
            businessEntity.AccountId = Guid.Parse(ComponentsRecordsInfo.GetSelectedRecord("Account"));
            businessEntity.ParentDivisionName = UIEntity.ParentDivisionName;
            businessEntity.Name = UIEntity.Name;
            return businessEntity;
        }

        public override SysCreate_Account_Division_Popup_Applet BusinessToUI(SysDivision businessEntity)
        {
            SysCreate_Account_Division_Popup_Applet UIEntity = base.BusinessToUI(businessEntity);
            UIEntity.ParentDivisionName = businessEntity.ParentDivisionName;
            UIEntity.Name = businessEntity.Name;
            return UIEntity;
        }

        public override IEnumerable<ValidationResult> UIValidate(GSAppContext context, IViewInfo viewInfo, SysCreate_Account_Division_Popup_Applet UIEntity, bool isNewRecord)
        {
            List<ValidationResult> result = base.UIValidate(context, viewInfo, UIEntity, isNewRecord).ToList();
            return result;
        }

        public override IEnumerable<ValidationResult> BUSUIValidate(GSAppContext context, SysDivision businessEntity, SysCreate_Account_Division_Popup_Applet UIEntity)
        {
            List<ValidationResult> result = base.BUSUIValidate(context, businessEntity, UIEntity).ToList();
            return result;
        }
	}
}
