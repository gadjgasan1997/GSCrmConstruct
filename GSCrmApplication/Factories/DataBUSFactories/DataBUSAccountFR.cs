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
using SysAccount_1 = GSCrmApplication.Models.TableModels.Account;
using SysAccount_2 = GSCrmApplication.Models.BusinessComponentModels.Account;

namespace GSCrmApplication.Factories.DataBUSFactories
{
	public class DataBUSAccountFR : DataBUSFactory<SysAccount_1, SysAccount_2, GSAppContext>
	{
        public override SysAccount_2 DataToBusiness(SysAccount_1 dataEntity, GSAppContext context)
        {
            SysAccount_2 businessEntity = base.DataToBusiness(dataEntity, context);
            businessEntity.KPP = dataEntity.KPP;
            businessEntity.OKPO = dataEntity.OKPO;
            businessEntity.Resident = dataEntity.Resident;
            businessEntity.INN = dataEntity.INN;
            businessEntity.Name = dataEntity.Name;
            businessEntity.OGRN = dataEntity.OGRN;
            return businessEntity;
        }
        public override SysAccount_1 BusinessToData(SysAccount_1 sysaccount_1, SysAccount_2 businessEntity, GSAppContext context, bool NewRecord)
        {
            SysAccount_1 dataEntity = base.BusinessToData(sysaccount_1, businessEntity, context, NewRecord);
            dataEntity.KPP = businessEntity.KPP;
            dataEntity.OKPO = businessEntity.OKPO;
            dataEntity.Resident = businessEntity.Resident;
            dataEntity.INN = businessEntity.INN;
            dataEntity.Name = businessEntity.Name;
            dataEntity.OGRN = businessEntity.OGRN;
            return dataEntity;
        }

        public override IEnumerable<ValidationResult> DataBUSValidate(SysAccount_1 dataEntity, SysAccount_2 businessEntity, IViewInfo viewInfo, GSAppContext context)
        {
            List<ValidationResult> result = base.DataBUSValidate(dataEntity, businessEntity, viewInfo, context).ToList();
            Applet currentApplet = viewInfo.CurrentPopupApplet ?? viewInfo.CurrentApplet;
            return result;
        }
	}
}
