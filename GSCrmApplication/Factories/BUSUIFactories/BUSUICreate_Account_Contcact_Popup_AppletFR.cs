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
using SysContact = GSCrmApplication.Models.BusinessComponentModels.Contact;
using SysCreate_Account_Contcact_Popup_Applet = GSCrmApplication.Models.AppletModels.Create_Account_Contcact_Popup_Applet;

namespace GSCrmApplication.Factories.BUSUIFactories
{
	public class BUSUICreate_Account_Contcact_Popup_AppletFR : BUSUIFactory<SysContact, SysCreate_Account_Contcact_Popup_Applet, GSAppContext>
	{
        public override SysContact UIToBusiness(SysCreate_Account_Contcact_Popup_Applet UIEntity, GSAppContext context, IViewInfo viewInfo, bool NewRecord)
        {
            SysContact businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, NewRecord);
            businessEntity.AccountId = Guid.Parse(ComponentsRecordsInfo.GetSelectedRecord("Account"));
            businessEntity.WorkNumber = UIEntity.WorkNumber;
            businessEntity.Email = UIEntity.Email;
            businessEntity.AccountName = UIEntity.AccountName;
            return businessEntity;
        }

        public override SysCreate_Account_Contcact_Popup_Applet BusinessToUI(SysContact businessEntity)
        {
            SysCreate_Account_Contcact_Popup_Applet UIEntity = base.BusinessToUI(businessEntity);
            UIEntity.WorkNumber = businessEntity.WorkNumber;
            UIEntity.Email = businessEntity.Email;
            UIEntity.AccountName = businessEntity.AccountName;
            return UIEntity;
        }

        public override IEnumerable<ValidationResult> UIValidate(GSAppContext context, IViewInfo viewInfo, SysCreate_Account_Contcact_Popup_Applet UIEntity, bool isNewRecord)
        {
            List<ValidationResult> result = base.UIValidate(context, viewInfo, UIEntity, isNewRecord).ToList();
            return result;
        }

        public override IEnumerable<ValidationResult> BUSUIValidate(GSAppContext context, SysContact businessEntity, SysCreate_Account_Contcact_Popup_Applet UIEntity)
        {
            List<ValidationResult> result = base.BUSUIValidate(context, businessEntity, UIEntity).ToList();
            return result;
        }
	}
}
