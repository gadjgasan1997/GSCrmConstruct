using System.Linq;
using GSCrmLibrary.Data;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Models.BusinessComponentModels;
using GSCrmLibrary.Factories.MainFactories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GSCrmLibrary.Models.AppletModels;

namespace GSCrmLibrary.Factories.DataBUSFactories
{
    public class DataBUSViewItemFR<TContext> : DataBUSFactory<ViewItem, BUSViewItem, TContext>
        where TContext : MainContext, new()
    {
        public override BUSViewItem DataToBusiness(ViewItem dataEntity, TContext context)
        {
            BUSViewItem businessEntity = base.DataToBusiness(dataEntity, context);
            Applet applet = context.Applets.AsNoTracking().FirstOrDefault(i => i.Id == dataEntity.AppletId);
            if (applet != null)
            {
                businessEntity.Applet = applet;
                businessEntity.AppletId = applet.Id;
                businessEntity.AppletName = applet.Name;
            }
            businessEntity.View = context.Views.AsNoTracking().FirstOrDefault(i => i.Id == dataEntity.ViewId);
            businessEntity.ViewId = dataEntity.ViewId;
            businessEntity.Autofocus = dataEntity.Autofocus;
            businessEntity.AutofocusRecord = dataEntity.AutofocusRecord;
            return businessEntity;
        }
        public override ViewItem BusinessToData(ViewItem viewItem, BUSViewItem businessEntity, TContext context, bool NewRecord)
        {
            ViewItem dataEntity = base.BusinessToData(viewItem, businessEntity, context, NewRecord);
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