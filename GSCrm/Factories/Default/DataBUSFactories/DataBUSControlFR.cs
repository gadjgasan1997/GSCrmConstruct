using GSCrm.Data;
using GSCrm.Models.Default.TableModels;
using GSCrm.Models.Default.BusinessComponentModels;
using Microsoft.EntityFrameworkCore;
using GSCrm.Factories.Default.MainFactories;
using System.Linq;
using System;
using Action = GSCrm.Models.Default.TableModels.Action;

namespace GSCrm.Factories.Default.DataBUSFactories
{
    public class DataBUSControlFR : MainDataBUSFR<Control, BUSControl>
    {
        public override BUSControl DataToBusiness(Control dataEntity, ApplicationContext context)
        {
            BUSControl businessEntity = base.DataToBusiness(dataEntity, context);
            businessEntity.Header = dataEntity.Header;
            businessEntity.CssClass = dataEntity.CssClass;

            // Icon
            Icon icon = context.Icons.FirstOrDefault(i => i.Id == dataEntity.IconId);
            if (icon != null)
            {
                businessEntity.IconId = icon.Id;
                businessEntity.IconName = icon.Name;
            }

            // Applet
            businessEntity.Applet = context.Applets.FirstOrDefault(i => i.Id == dataEntity.AppletId);
            businessEntity.AppletId = dataEntity.AppletId;

            // Field
            Field field = context.Fields.FirstOrDefault(i => i.Id == dataEntity.FieldId);
            if (field != null)
            {
                businessEntity.Field = field;
                businessEntity.FieldId = field.Id;
                businessEntity.FieldName = field.Name;
            }

            // Action
            Action action = context.Actions.FirstOrDefault(i => i.Id == dataEntity.ActionId);
            if (action != null)
            {
                businessEntity.Action = action;
                businessEntity.ActionId = action.Id;
                businessEntity.ActionName = action.Name;
            }
            return businessEntity;
        }
        public override Control BusinessToData(BUSControl businessEntity, DbSet<Control> entityDBSet, bool NewRecord)
        {
            Control dataEntity = base.BusinessToData(businessEntity, entityDBSet, NewRecord);
            dataEntity.Header = businessEntity.Header;
            dataEntity.IconId = businessEntity.IconId;
            dataEntity.CssClass = businessEntity.CssClass;
            dataEntity.Applet = businessEntity.Applet;
            dataEntity.AppletId = businessEntity.AppletId;
            dataEntity.FieldId = businessEntity.FieldId;
            dataEntity.Action = businessEntity.Action;
            dataEntity.ActionId = businessEntity.ActionId;
            return dataEntity;
        }
    }
}
