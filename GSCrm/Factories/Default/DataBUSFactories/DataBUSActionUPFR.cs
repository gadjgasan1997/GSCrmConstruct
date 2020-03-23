using GSCrm.Data;
using GSCrm.Models.Default.BusinessComponentModels;
using GSCrm.Models.Default.TableModels;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using GSCrm.Factories.Default.MainFactories;

namespace GSCrm.Factories.Default.DataBUSFactories
{
    public class DataBUSActionUPFR : MainUPDataBUSFR<ActionUP, BUSActionUP>
    {
        public override BUSActionUP DataToBusiness(ActionUP dataEntity, ApplicationContext context)
        {
            BUSActionUP businessEntity = base.DataToBusiness(dataEntity, context);
            businessEntity.Action = context.Actions.FirstOrDefault(i => i.Id == dataEntity.ActionId);
            businessEntity.ActionId = dataEntity.ActionId;
            return businessEntity;
        }
        public override ActionUP BusinessToData(BUSActionUP businessEntity, DbSet<ActionUP> entityDBSet, bool NewRecord)
        {
            ActionUP dataEntity = base.BusinessToData(businessEntity, entityDBSet, NewRecord);
            dataEntity.Action = businessEntity.Action;
            dataEntity.ActionId = businessEntity.ActionId;
            return dataEntity;
        }
    }
}
