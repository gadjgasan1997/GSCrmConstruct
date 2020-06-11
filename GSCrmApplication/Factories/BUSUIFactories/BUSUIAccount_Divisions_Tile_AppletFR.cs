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
using SysAccount_Divisions_Tile_Applet = GSCrmApplication.Models.AppletModels.Account_Divisions_Tile_Applet;

namespace GSCrmApplication.Factories.BUSUIFactories
{
	public class BUSUIAccount_Divisions_Tile_AppletFR : BUSUIFactory<SysDivision, SysAccount_Divisions_Tile_Applet, GSAppContext>
	{
        public override SysDivision UIToBusiness(SysAccount_Divisions_Tile_Applet UIEntity, GSAppContext context, IViewInfo viewInfo, bool NewRecord)
        {
            SysDivision businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, NewRecord);
            businessEntity.AccountId = Guid.Parse(ComponentsRecordsInfo.GetSelectedRecord("Account"));
            businessEntity.Name = UIEntity.Name;
            businessEntity.ParentDivisionName = UIEntity.ParentDivisionName;
            return businessEntity;
        }

        public override SysAccount_Divisions_Tile_Applet BusinessToUI(SysDivision businessEntity)
        {
            SysAccount_Divisions_Tile_Applet UIEntity = base.BusinessToUI(businessEntity);
            UIEntity.Name = businessEntity.Name;
            UIEntity.ParentDivisionName = businessEntity.ParentDivisionName;
            return UIEntity;
        }

        public override IEnumerable<ValidationResult> UIValidate(GSAppContext context, IViewInfo viewInfo, SysAccount_Divisions_Tile_Applet UIEntity, bool isNewRecord)
        {
            List<ValidationResult> result = base.UIValidate(context, viewInfo, UIEntity, isNewRecord).ToList();
            return result;
        }

        public override IEnumerable<ValidationResult> BUSUIValidate(GSAppContext context, SysDivision businessEntity, SysAccount_Divisions_Tile_Applet UIEntity)
        {
            List<ValidationResult> result = base.BUSUIValidate(context, businessEntity, UIEntity).ToList();
            return result;
        }
	}
}
