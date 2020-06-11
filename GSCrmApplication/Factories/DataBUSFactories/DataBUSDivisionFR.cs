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
using SysDivision_1 = GSCrmApplication.Models.TableModels.Division;
using SysDivision_2 = GSCrmApplication.Models.BusinessComponentModels.Division;

namespace GSCrmApplication.Factories.DataBUSFactories
{
	public class DataBUSDivisionFR : DataBUSFactory<SysDivision_1, SysDivision_2, GSAppContext>
	{
        public override SysDivision_2 DataToBusiness(SysDivision_1 dataEntity, GSAppContext context)
        {
            SysDivision_2 businessEntity = base.DataToBusiness(dataEntity, context);
            businessEntity.AccountId = dataEntity.AccountId;
            businessEntity.Name = dataEntity.Name;
            businessEntity.ParentDivisionId = dataEntity.ParentDivisionId;
            Division ParentDivision = context.Division.AsNoTracking().FirstOrDefault(i => i.Id == businessEntity.ParentDivisionId);
            if (ParentDivision != null)
            {
                businessEntity.ParentDivisionName = ParentDivision.Name;
            }
            return businessEntity;
        }
        public override SysDivision_1 BusinessToData(SysDivision_1 sysdivision_1, SysDivision_2 businessEntity, GSAppContext context, bool NewRecord)
        {
            SysDivision_1 dataEntity = base.BusinessToData(sysdivision_1, businessEntity, context, NewRecord);
            dataEntity.AccountId = businessEntity.AccountId;
            dataEntity.Name = businessEntity.Name;
            dataEntity.ParentDivisionId = businessEntity.ParentDivisionId;
            return dataEntity;
        }

        public override IEnumerable<ValidationResult> DataBUSValidate(SysDivision_1 dataEntity, SysDivision_2 businessEntity, IViewInfo viewInfo, GSAppContext context)
        {
            List<ValidationResult> result = base.DataBUSValidate(dataEntity, businessEntity, viewInfo, context).ToList();
            Applet currentApplet = viewInfo.CurrentPopupApplet ?? viewInfo.CurrentApplet;
            return result;
        }
	}
}
