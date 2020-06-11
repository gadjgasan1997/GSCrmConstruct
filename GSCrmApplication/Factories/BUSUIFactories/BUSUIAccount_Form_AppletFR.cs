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
using SysAccount = GSCrmApplication.Models.BusinessComponentModels.Account;
using SysAccount_Form_Applet = GSCrmApplication.Models.AppletModels.Account_Form_Applet;

namespace GSCrmApplication.Factories.BUSUIFactories
{
	public class BUSUIAccount_Form_AppletFR : BUSUIFactory<SysAccount, SysAccount_Form_Applet, GSAppContext>
	{
        public override SysAccount UIToBusiness(SysAccount_Form_Applet UIEntity, GSAppContext context, IViewInfo viewInfo, bool NewRecord)
        {
            SysAccount businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, NewRecord);
            businessEntity.INN = UIEntity.INN;
            businessEntity.OGRN = UIEntity.OGRN;
            businessEntity.Name = UIEntity.Name;
            businessEntity.Resident = UIEntity.Resident;
            businessEntity.OKPO = UIEntity.OKPO;
            businessEntity.KPP = UIEntity.KPP;
            return businessEntity;
        }

        public override SysAccount_Form_Applet BusinessToUI(SysAccount businessEntity)
        {
            SysAccount_Form_Applet UIEntity = base.BusinessToUI(businessEntity);
            UIEntity.INN = businessEntity.INN;
            UIEntity.OGRN = businessEntity.OGRN;
            UIEntity.Name = businessEntity.Name;
            UIEntity.Resident = businessEntity.Resident;
            UIEntity.OKPO = businessEntity.OKPO;
            UIEntity.KPP = businessEntity.KPP;
            return UIEntity;
        }

        public override IEnumerable<ValidationResult> UIValidate(GSAppContext context, IViewInfo viewInfo, SysAccount_Form_Applet UIEntity, bool isNewRecord)
        {
            List<ValidationResult> result = base.UIValidate(context, viewInfo, UIEntity, isNewRecord).ToList();
            return result;
        }

        public override IEnumerable<ValidationResult> BUSUIValidate(GSAppContext context, SysAccount businessEntity, SysAccount_Form_Applet UIEntity)
        {
            List<ValidationResult> result = base.BUSUIValidate(context, businessEntity, UIEntity).ToList();
            return result;
        }
	}
}
