using GSCrm.Data;
using GSCrm.Models.Default.TableModels;
using GSCrm.Models.Default.BusinessComponentModels;
using Microsoft.EntityFrameworkCore;
using GSCrm.Factories.Default.MainFactories;
using System.Linq;

namespace GSCrm.Factories.Default.DataBUSFactories
{
    public class DataBUSColumnFR : MainDataBUSFR<Column, BUSColumn>
    {
        public override BUSColumn DataToBusiness(Column dataEntity, ApplicationContext context)
        {
            BUSColumn businessEntity = base.DataToBusiness(dataEntity, context);
            businessEntity.Header = dataEntity.Header;
            businessEntity.IconId = dataEntity.IconId;

            // Applet
            Applet applet = context.Applets.FirstOrDefault(i => i.Id == dataEntity.AppletId);
            businessEntity.Applet = applet;
            businessEntity.AppletId = applet.Id;

            // Field
            Field field = context.Fields.FirstOrDefault(i => i.Id == dataEntity.FieldId);
            if (field != null)
            {
                businessEntity.Field = field;
                businessEntity.FieldId = field.Id;
                businessEntity.FieldName = field.Name;
            }
            return businessEntity;
        }
        public override Column BusinessToData(BUSColumn businessEntity, DbSet<Column> entityDBSet, bool NewRecord)
        {
            Column dataEntity = base.BusinessToData(businessEntity, entityDBSet, NewRecord);
            dataEntity.Header = businessEntity.Header;
            dataEntity.IconId = businessEntity.IconId;
            dataEntity.Applet = businessEntity.Applet;
            dataEntity.AppletId = businessEntity.AppletId;
            dataEntity.FieldId = businessEntity.FieldId;
            return dataEntity;
        }
    }
}
