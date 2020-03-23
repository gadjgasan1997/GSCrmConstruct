using GSCrm.Data;
using GSCrm.Models.Default.TableModels;
using GSCrm.Models.Default.BusinessComponentModels;
using System.Linq;
using GSCrm.Factories.Default.MainFactories;
using Microsoft.EntityFrameworkCore;

namespace GSCrm.Factories.Default.DataBUSFactories
{
    public class DataBUSJoinSpecificationFR : MainDataBUSFR<JoinSpecification, BUSJoinSpecification>
    {
        public override BUSJoinSpecification DataToBusiness(JoinSpecification dataEntity, ApplicationContext context)
        {
            BUSJoinSpecification businessEntity = base.DataToBusiness(dataEntity, context);

            // Join
            Join join = context.Joins
                    .Include(t => t.Table)
                        .ThenInclude(tc => tc.TableColumns)
                    .FirstOrDefault(i => i.Id == dataEntity.JoinId);
            businessEntity.Join = join;
            businessEntity.JoinId = join.Id;

            // BusComp
            BusComp busComp = context.BusinessComponents
                .Include(f => f.Fields)
                .FirstOrDefault(i => i.Id == join.BusCompId);

            // Source field
            Field field = busComp.Fields.FirstOrDefault(i => i.Id == dataEntity.SourceFieldId);
            businessEntity.SourceField = field;
            businessEntity.SourceFieldId = field.Id;
            businessEntity.SourceFieldName = field.Name;

            // Destination column
            Table table = join.Table;
            TableColumn destinationColumn = table.TableColumns.FirstOrDefault(i => i.Id == dataEntity.DestinationColumnId);
            businessEntity.DestinationColumn = destinationColumn;
            businessEntity.DestinationColumnId = destinationColumn.Id;
            businessEntity.DestinationColumnName = destinationColumn.Name;
            return businessEntity;
        }
        public override JoinSpecification BusinessToData(BUSJoinSpecification businessEntity, DbSet<JoinSpecification> entityDBSet, bool NewRecord)
        {
            JoinSpecification dataEntity = base.BusinessToData(businessEntity, entityDBSet, NewRecord);
            dataEntity.Join = businessEntity.Join;
            dataEntity.JoinId = businessEntity.JoinId;
            dataEntity.SourceField = businessEntity.SourceField;
            dataEntity.SourceFieldId = businessEntity.SourceFieldId;
            dataEntity.DestinationColumn = businessEntity.DestinationColumn;
            dataEntity.DestinationColumnId = businessEntity.DestinationColumnId;
            return dataEntity;
        }
    }
}
