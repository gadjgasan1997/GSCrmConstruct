using GSCrm.Data;
using GSCrm.Models.Default.BusinessComponentModels;
using GSCrm.Models.Default.TableModels;
using GSCrm.Factories.Default.MainFactories;
using Microsoft.EntityFrameworkCore;

namespace GSCrm.Factories.Default.DataBUSFactories
{
    public class DataBUSIconFR : MainDataBUSFR<Icon, BUSIcon>
    {
        public override BUSIcon DataToBusiness(Icon dataEntity, ApplicationContext context)
        {
            BUSIcon businessEntity = base.DataToBusiness(dataEntity, context);
            businessEntity.ImgPath = dataEntity.ImgPath;
            businessEntity.CssClass = dataEntity.CssClass;
            return businessEntity;
        }
        public override Icon BusinessToData(BUSIcon businessEntity, DbSet<Icon> entityDBSet, bool NewRecord)
        {
            Icon dataEntity = base.BusinessToData(businessEntity, entityDBSet, NewRecord);
            dataEntity.ImgPath = businessEntity.ImgPath;
            dataEntity.CssClass = businessEntity.CssClass;
            return dataEntity;
        }
    }
}
