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
using SysAccount_Addresses_Tile_Applet = GSCrmApplication.Models.AppletModels.Account_Addresses_Tile_Applet;

namespace GSCrmApplication.Factories.BUSUIFactories
{
	public class BUSUIAccount_Addresses_Tile_AppletFR : BUSUIFactory<SysAddress, SysAccount_Addresses_Tile_Applet, GSAppContext>
	{
        public override SysAddress UIToBusiness(SysAccount_Addresses_Tile_Applet UIEntity, GSAppContext context, IViewInfo viewInfo, bool NewRecord)
        {
            SysAddress businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, NewRecord);
            businessEntity.AccountId = Guid.Parse(ComponentsRecordsInfo.GetSelectedRecord("Account"));
            businessEntity.FullAddress = UIEntity.FullAddress;
            return businessEntity;
        }

        public override SysAccount_Addresses_Tile_Applet BusinessToUI(SysAddress businessEntity)
        {
            SysAccount_Addresses_Tile_Applet UIEntity = base.BusinessToUI(businessEntity);
            UIEntity.FullAddress = businessEntity.FullAddress;
            return UIEntity;
        }

        public override IEnumerable<ValidationResult> UIValidate(GSAppContext context, IViewInfo viewInfo, SysAccount_Addresses_Tile_Applet UIEntity, bool isNewRecord)
        {
            List<ValidationResult> result = base.UIValidate(context, viewInfo, UIEntity, isNewRecord).ToList();
            return result;
        }

        public override IEnumerable<ValidationResult> BUSUIValidate(GSAppContext context, SysAddress businessEntity, SysAccount_Addresses_Tile_Applet UIEntity)
        {
            List<ValidationResult> result = base.BUSUIValidate(context, businessEntity, UIEntity).ToList();
            return result;
        }
	}
}
