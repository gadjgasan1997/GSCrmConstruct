using System.Linq;
using Microsoft.EntityFrameworkCore;
using GSCrmLibrary.Data;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Models.BusinessComponentModels;
using GSCrmLibrary.Factories.MainFactories;

namespace GSCrmLibrary.Factories.DataBUSFactories
{
    public class DataBUSControlFR<TContext> : DataBUSFactory<Control, BUSControl, TContext>
        where TContext : MainContext, new()
    {
        public override BUSControl DataToBusiness(Control dataEntity, TContext context)
        {
            BUSControl businessEntity = base.DataToBusiness(dataEntity, context);

            // Applet
            Applet applet = context.Applets
                .AsNoTracking()
                .Select(a => new
                {
                    id = a.Id,
                    name = a.Name,
                    busComp = a.BusComp,
                    busCompId = a.BusCompId
                })
                .Select(a => new Applet
                {
                    Id = a.id,
                    Name = a.name,
                    BusComp = a.busComp,
                    BusCompId = a.busCompId
                })
                .FirstOrDefault(i => i.Id == dataEntity.AppletId);

            businessEntity.Applet = applet;
            businessEntity.AppletId = applet.Id;
            businessEntity.BusComp = applet.BusComp;
            businessEntity.BusCompId = applet.BusCompId;

            // Icon
            Icon icon = context.Icons.AsNoTracking().FirstOrDefault(i => i.Id == dataEntity.IconId);
            if (icon != null)
            {
                businessEntity.IconId = icon.Id;
                businessEntity.IconName = icon.Name;
            }

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

            // Ридонли на филде приоритетнее ридонли на контроле, поэтому вначале проверяется филда. Затем, если на ней нет ридонли, то свойство берется с контрола
            if (field != null && field.Readonly)
                businessEntity.Readonly = true;
            else businessEntity.Readonly = dataEntity.Readonly;

            businessEntity.ActionType = dataEntity.ActionType;
            businessEntity.Header = dataEntity.Header;
            businessEntity.Type = dataEntity.Type;
            businessEntity.Required = dataEntity.Required;
            businessEntity.CssClass = dataEntity.CssClass;
            return businessEntity;
        }
        public override Control BusinessToData(Control control, BUSControl businessEntity, TContext context, bool NewRecord)
        {
            Control dataEntity = base.BusinessToData(control, businessEntity, context, NewRecord);
            dataEntity.IconId = businessEntity.IconId;
            dataEntity.CssClass = businessEntity.CssClass;
            dataEntity.Applet = businessEntity.Applet;
            dataEntity.AppletId = businessEntity.AppletId;
            dataEntity.FieldId = businessEntity.FieldId;
            dataEntity.ActionType = businessEntity.ActionType;
            dataEntity.Header = businessEntity.Header;
            dataEntity.Type = businessEntity.Type;
            dataEntity.Required = businessEntity.Required;
            dataEntity.Readonly = businessEntity.Readonly;
            return dataEntity;
        }
        public override void OnRecordDelete(Control recordToDelete, DbSet<Control> entities, TContext context)
        {
            context.ControlUPs.RemoveRange(context.ControlUPs.AsNoTracking().Where(i => i.ControlId == recordToDelete.Id));
            base.OnRecordDelete(recordToDelete, entities, context);
        }
    }
}
