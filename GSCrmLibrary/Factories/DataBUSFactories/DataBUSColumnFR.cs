using System.Linq;
using Microsoft.EntityFrameworkCore;
using GSCrmLibrary.Data;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Models.BusinessComponentModels;
using GSCrmLibrary.Factories.MainFactories;

namespace GSCrmLibrary.Factories.DataBUSFactories
{
    public class DataBUSColumnFR<TContext> : DataBUSFactory<Column, BUSColumn, TContext>
        where TContext : MainContext, new()
    {
        public override BUSColumn DataToBusiness(Column dataEntity, TContext context)
        {
            BUSColumn businessEntity = base.DataToBusiness(dataEntity, context);
            businessEntity.IconId = dataEntity.IconId;

            // Applet
            Applet applet = context.Applets.AsNoTracking().FirstOrDefault(i => i.Id == dataEntity.AppletId);
            businessEntity.Applet = applet;
            businessEntity.AppletId = applet.Id;
            businessEntity.BusComp = applet.BusComp;
            businessEntity.BusCompId = applet.BusCompId;

            // Field
            Field field = context.Fields.AsNoTracking().FirstOrDefault(i => i.Id == dataEntity.FieldId);
            if (field != null)
            {
                businessEntity.Field = field;
                businessEntity.FieldId = field.Id;
                businessEntity.FieldName = field.Name;
            }
            else
            {
                dataEntity.Field = null;
                dataEntity.FieldId = null;
            }
            
            // Ридонли на филде приоритетнее ридонли на колонке, поэтому вначале проверяется филда. Затем, если на ней нет ридонли, то свойство берется с колонки
            if (field != null && field.Readonly)
                businessEntity.Readonly = true;
            else businessEntity.Readonly = dataEntity.Readonly;

            businessEntity.ActionType = dataEntity.ActionType;
            businessEntity.Header = dataEntity.Header;
            businessEntity.Type = dataEntity.Type;
            businessEntity.Required = dataEntity.Required;
            return businessEntity;
        }
        public override Column BusinessToData(Column column, BUSColumn businessEntity, TContext context, bool NewRecord)
        {
            Column dataEntity = base.BusinessToData(column, businessEntity, context, NewRecord);
            dataEntity.IconId = businessEntity.IconId;
            dataEntity.Applet = businessEntity.Applet;
            dataEntity.AppletId = businessEntity.AppletId;
            dataEntity.Field = businessEntity.Field;
            dataEntity.FieldId = businessEntity.FieldId;
            dataEntity.Header = businessEntity.Header;
            dataEntity.Type = businessEntity.Type;
            dataEntity.Required = businessEntity.Required;
            dataEntity.Readonly = businessEntity.Readonly;
            return dataEntity;
        }
        public override void OnRecordDelete(Column recordToDelete, DbSet<Column> entities, TContext context)
        {
            context.ColumnUPs.RemoveRange(context.ColumnUPs.AsNoTracking().Where(i => i.ColumnId == recordToDelete.Id));
            base.OnRecordDelete(recordToDelete, entities, context);
        }
    }
}
