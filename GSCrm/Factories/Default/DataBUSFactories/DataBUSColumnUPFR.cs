using GSCrm.Data;
using GSCrm.Models.Default.BusinessComponentModels;
using GSCrm.Models.Default.TableModels;
using Microsoft.EntityFrameworkCore;
using GSCrm.Factories.Default.MainFactories;
using System.Linq;

namespace GSCrm.Factories.Default.DataBUSFactories
{
    public class DataBUSColumnUPFR : MainUPDataBUSFR<ColumnUP, BUSColumnUP>
    {
        public override BUSColumnUP DataToBusiness(ColumnUP dataEntity, ApplicationContext context)
        {
            BUSColumnUP businessEntity = base.DataToBusiness(dataEntity, context);
            businessEntity.ColumnId = dataEntity.ColumnId;
            businessEntity.Column = context.Columns.FirstOrDefault(i => i.Id == businessEntity.ColumnId);
            return businessEntity;
        }
        public override ColumnUP BusinessToData(BUSColumnUP businessEntity, DbSet<ColumnUP> entityDBSet, bool NewRecord)
        {
            ColumnUP dataEntity = base.BusinessToData(businessEntity, entityDBSet, NewRecord);
            dataEntity.Column = businessEntity.Column;
            dataEntity.ColumnId = businessEntity.ColumnId;
            return dataEntity;
        }
    }
}
