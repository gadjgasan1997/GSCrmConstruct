using GSCrmLibrary.Data;
using GSCrmLibrary.Models.BusinessComponentModels;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Factories.MainFactories;
using System.Linq;

namespace GSCrmLibrary.Factories.DataBUSFactories
{
    public class DataBUSIconFR<TContext> : DataBUSFactory<Icon, BUSIcon, TContext>
        where TContext : MainContext, new()
    {
        public override BUSIcon DataToBusiness(Icon dataEntity, TContext context)
        {
            BUSIcon businessEntity = base.DataToBusiness(dataEntity, context);
            businessEntity.ImgPath = dataEntity.ImgPath;
            businessEntity.CssClass = dataEntity.CssClass;
            return businessEntity;
        }
        public override Icon BusinessToData(Icon icon, BUSIcon businessEntity,TContext context, bool NewRecord)
        {
            Icon dataEntity = base.BusinessToData(icon, businessEntity, context, NewRecord);
            dataEntity.ImgPath = businessEntity.ImgPath;
            dataEntity.CssClass = businessEntity.CssClass;
            return dataEntity;
        }
    }
}
