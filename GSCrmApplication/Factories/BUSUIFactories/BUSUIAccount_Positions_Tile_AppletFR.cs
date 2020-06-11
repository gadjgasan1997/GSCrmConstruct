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
using SysAccount_Positions_Tile_Applet = GSCrmApplication.Models.AppletModels.Account_Positions_Tile_Applet;

namespace GSCrmApplication.Factories.BUSUIFactories
{
	public class BUSUIAccount_Positions_Tile_AppletFR : BUSUIFactory<SysPosition, SysAccount_Positions_Tile_Applet, GSAppContext>
	{
        public override SysPosition UIToBusiness(SysAccount_Positions_Tile_Applet UIEntity, GSAppContext context, IViewInfo viewInfo, bool NewRecord)
        {
            SysPosition businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, NewRecord);
            businessEntity.AccountId = Guid.Parse(ComponentsRecordsInfo.GetSelectedRecord("Account"));
            businessEntity.ParentPositionName = UIEntity.ParentPositionName;
            businessEntity.PrimaryEmployeeFullName = UIEntity.PrimaryEmployeeFullName;
            businessEntity.Name = UIEntity.Name;
            return businessEntity;
        }

        public override SysAccount_Positions_Tile_Applet BusinessToUI(SysPosition businessEntity)
        {
            SysAccount_Positions_Tile_Applet UIEntity = base.BusinessToUI(businessEntity);
            UIEntity.ParentPositionName = businessEntity.ParentPositionName;
            UIEntity.PrimaryEmployeeFullName = businessEntity.PrimaryEmployeeFullName;
            UIEntity.Name = businessEntity.Name;
            return UIEntity;
        }

        public override IEnumerable<ValidationResult> UIValidate(GSAppContext context, IViewInfo viewInfo, SysAccount_Positions_Tile_Applet UIEntity, bool isNewRecord)
        {
            List<ValidationResult> result = base.UIValidate(context, viewInfo, UIEntity, isNewRecord).ToList();
            return result;
        }

        public override IEnumerable<ValidationResult> BUSUIValidate(GSAppContext context, SysPosition businessEntity, SysAccount_Positions_Tile_Applet UIEntity)
        {
            List<ValidationResult> result = base.BUSUIValidate(context, businessEntity, UIEntity).ToList();
            return result;
        }
	}
}
