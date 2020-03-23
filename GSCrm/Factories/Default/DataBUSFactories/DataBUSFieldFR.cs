using GSCrm.Data;
using GSCrm.Models.Default.TableModels;
using GSCrm.Models.Default.BusinessComponentModels;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using GSCrm.Factories.Default.MainFactories;

namespace GSCrm.Factories.Default.DataBUSFactories
{
    public class DataBUSFieldFR : MainDataBUSFR<Field, BUSField>
    {
        public override BUSField DataToBusiness(Field dataEntity, ApplicationContext context)
        {
            BUSField businessEntity = base.DataToBusiness(dataEntity, context);

            // BusComp
            BusComp busComp = context.BusinessComponents
                .Include(t => t.Table)
                    .ThenInclude(tc => tc.TableColumns)
                .FirstOrDefault(i => i.Id == dataEntity.BusCompId);
            businessEntity.BusComp = busComp;
            businessEntity.BusCompId = busComp.Id;
            businessEntity.BusCompName = busComp.Name;

            // Join
            Join join = context.Joins
                .Include(t => t.Table)
                    .ThenInclude(tc => tc.TableColumns)
                .FirstOrDefault(i => i.Id == dataEntity.JoinId);

            if (join != null)
            {
                businessEntity.Join = join;
                businessEntity.JoinId = join.Id;
                businessEntity.JoinName = join.Name;
            }

            // Table column
            if (busComp.Table != null || join?.Table != null)
            {
                // Колонка в таблице может быть либо с той таблицы, на которой основана бк, либо с той таблицы, на которой основан join
                Table table = join?.Table == null ? busComp.Table : join.Table;
                if (table != null && table.TableColumns != null)
                {
                    TableColumn tableColumn = table.TableColumns.FirstOrDefault(i => i.Id == dataEntity.TableColumnId);
                    if (tableColumn != null)
                    {
                        businessEntity.TableColumn = tableColumn;
                        businessEntity.TableColumnId = tableColumn.Id;
                        businessEntity.TableColumnName = tableColumn.Name;
                    }
                }
            }

            // PickList
            if (dataEntity.PickListId != null)
            {
                PL pickList = context.PickLists.FirstOrDefault(i => i.Id == dataEntity.PickListId);
                if (pickList != null)
                {
                    businessEntity.PickListId = dataEntity.PickListId;
                    businessEntity.PickListName = pickList.Name;
                }
            }
            return businessEntity;
        }
        public override Field BusinessToData(BUSField businessEntity, DbSet<Field> entityDBSet, bool NewRecord)
        {
            Field dataEntity = base.BusinessToData(businessEntity, entityDBSet, NewRecord);
            dataEntity.BusComp = businessEntity.BusComp;
            dataEntity.BusCompId = businessEntity.BusCompId;
            dataEntity.Join = businessEntity.Join;
            dataEntity.JoinId = businessEntity.JoinId;
            dataEntity.PickListId = businessEntity.PickListId;
            dataEntity.TableColumnId = businessEntity.TableColumnId;
            return dataEntity;
        }
    }
}
