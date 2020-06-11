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
using SysAccount_Tile_Applet = GSCrmApplication.Models.AppletModels.Account_Tile_Applet;

namespace GSCrmApplication.Factories.BUSUIFactories
{
	public class BUSUIAccount_Tile_AppletFR : BUSUIFactory<SysAccount, SysAccount_Tile_Applet, GSAppContext>
	{
        public override SysAccount UIToBusiness(SysAccount_Tile_Applet UIEntity, GSAppContext context, IViewInfo viewInfo, bool NewRecord)
        {
            SysAccount businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, NewRecord);
            businessEntity.OGRN = UIEntity.OGRN;
            businessEntity.KPP = UIEntity.KPP;
            businessEntity.INN = UIEntity.INN;
            businessEntity.Resident = UIEntity.Resident;
            businessEntity.OKPO = UIEntity.OKPO;
            businessEntity.Name = UIEntity.Name;
            return businessEntity;
        }

        public override SysAccount_Tile_Applet BusinessToUI(SysAccount businessEntity)
        {
            SysAccount_Tile_Applet UIEntity = base.BusinessToUI(businessEntity);
            UIEntity.OGRN = businessEntity.OGRN;
            UIEntity.KPP = businessEntity.KPP;
            UIEntity.INN = businessEntity.INN;
            UIEntity.Resident = businessEntity.Resident;
            UIEntity.OKPO = businessEntity.OKPO;
            UIEntity.Name = businessEntity.Name;
            return UIEntity;
        }

        public override IEnumerable<ValidationResult> UIValidate(GSAppContext context, IViewInfo viewInfo, SysAccount_Tile_Applet UIEntity, bool isNewRecord)
        {
            List<ValidationResult> result = base.UIValidate(context, viewInfo, UIEntity, isNewRecord).ToList();
            return result;
        }

        public override IEnumerable<ValidationResult> BUSUIValidate(GSAppContext context, SysAccount businessEntity, SysAccount_Tile_Applet UIEntity)
        {
            List<ValidationResult> result = base.BUSUIValidate(context, businessEntity, UIEntity).ToList();
            return result;
        }
	}
}
