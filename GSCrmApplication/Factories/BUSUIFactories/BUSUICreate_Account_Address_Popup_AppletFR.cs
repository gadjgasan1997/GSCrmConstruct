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
using SysAddress = GSCrmApplication.Models.BusinessComponentModels.Address;
using SysCreate_Account_Address_Popup_Applet = GSCrmApplication.Models.AppletModels.Create_Account_Address_Popup_Applet;

namespace GSCrmApplication.Factories.BUSUIFactories
{
	public class BUSUICreate_Account_Address_Popup_AppletFR : BUSUIFactory<SysAddress, SysCreate_Account_Address_Popup_Applet, GSAppContext>
	{
        public override SysAddress UIToBusiness(SysCreate_Account_Address_Popup_Applet UIEntity, GSAppContext context, IViewInfo viewInfo, bool NewRecord)
        {
            SysAddress businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, NewRecord);
            businessEntity.AccountId = Guid.Parse(ComponentsRecordsInfo.GetSelectedRecord("Account"));
            businessEntity.Street = UIEntity.Street;
            businessEntity.Country = UIEntity.Country;
            businessEntity.House = UIEntity.House;
            businessEntity.City = UIEntity.City;
            return businessEntity;
        }

        public override SysCreate_Account_Address_Popup_Applet BusinessToUI(SysAddress businessEntity)
        {
            SysCreate_Account_Address_Popup_Applet UIEntity = base.BusinessToUI(businessEntity);
            UIEntity.Street = businessEntity.Street;
            UIEntity.Country = businessEntity.Country;
            UIEntity.House = businessEntity.House;
            UIEntity.City = businessEntity.City;
            return UIEntity;
        }

        public override IEnumerable<ValidationResult> UIValidate(GSAppContext context, IViewInfo viewInfo, SysCreate_Account_Address_Popup_Applet UIEntity, bool isNewRecord)
        {
            List<ValidationResult> result = base.UIValidate(context, viewInfo, UIEntity, isNewRecord).ToList();
            return result;
        }

        public override IEnumerable<ValidationResult> BUSUIValidate(GSAppContext context, SysAddress businessEntity, SysCreate_Account_Address_Popup_Applet UIEntity)
        {
            List<ValidationResult> result = base.BUSUIValidate(context, businessEntity, UIEntity).ToList();
            return result;
        }
	}
}
