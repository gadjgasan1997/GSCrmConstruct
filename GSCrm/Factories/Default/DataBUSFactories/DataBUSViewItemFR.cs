using GSCrm.Data;
using GSCrm.Models.Default.TableModels;
using GSCrm.Models.Default.BusinessComponentModels;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using GSCrm.Factories.Default.MainFactories;

namespace GSCrm.Factories.Default.DataBUSFactories
{
    public class DataBUSViewItemFR : MainDataBUSFR<ViewItem, BUSViewItem>
    {
        public override BUSViewItem DataToBusiness(ViewItem dataEntity, ApplicationContext context)
        {
            BUSViewItem businessEntity = base.DataToBusiness(dataEntity, context);
            Applet applet = context.Applets.FirstOrDefault(i => i.Id == dataEntity.AppletId);
            businessEntity.Applet = applet;
            businessEntity.AppletId = applet.Id;
            businessEntity.AppletName = applet.Name;
            businessEntity.Type = applet.Type;
            businessEntity.View = context.Views.FirstOrDefault(i => i.Id == dataEntity.ViewId);
            businessEntity.ViewId = dataEntity.ViewId;
            businessEntity.Autofocus = dataEntity.Autofocus;
            businessEntity.AutofocusRecord = dataEntity.AutofocusRecord;
            return businessEntity;
        }
        public override ViewItem BusinessToData(BUSViewItem businessEntity, DbSet<ViewItem> entityDBSet, bool NewRecord)
        {
            ViewItem dataEntity = base.BusinessToData(businessEntity, entityDBSet, NewRecord);
            dataEntity.Applet = businessEntity.Applet;
            dataEntity.AppletId = businessEntity.AppletId;
            dataEntity.View = businessEntity.View;
            dataEntity.ViewId = businessEntity.ViewId;
            dataEntity.Autofocus = businessEntity.Autofocus;
            dataEntity.AutofocusRecord = businessEntity.AutofocusRecord;
            return dataEntity;
        }
    }
}