using System.Linq;
using GSCrmLibrary.Data;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Models.BusinessComponentModels;
using GSCrmLibrary.Factories.MainFactories;
using Microsoft.EntityFrameworkCore;

namespace GSCrmLibrary.Factories.DataBUSFactories
{
    public class DataBUSApplicationItemFR<TContext> : DataBUSFactory<ApplicationItem, BUSApplicationItem, TContext>
        where TContext : MainContext, new()
    {
        public override BUSApplicationItem DataToBusiness(ApplicationItem dataEntity, TContext context)
        {
            BUSApplicationItem businessEntity = base.DataToBusiness(dataEntity, context);
            businessEntity.Application = dataEntity.Application;
            businessEntity.ApplicationId = dataEntity.ApplicationId;

            Screen screen = context.Screens.AsNoTracking().FirstOrDefault(i => i.Id == dataEntity.ScreenId);
            businessEntity.Screen = screen;
            if (screen != null)
            {
                businessEntity.ScreenId = screen.Id;
                businessEntity.ScreenName = screen.Name;
            }
            return businessEntity;
        }
        public override ApplicationItem BusinessToData(ApplicationItem applicationItem, BUSApplicationItem businessEntity, TContext context, bool NewRecord)
        {
            ApplicationItem dataEntity = base.BusinessToData(applicationItem, businessEntity, context, NewRecord);
            dataEntity.Application = businessEntity.Application;
            dataEntity.ApplicationId = businessEntity.ApplicationId;
            dataEntity.Screen = businessEntity.Screen;
            dataEntity.ScreenId = businessEntity.ScreenId;
            return dataEntity;
        }
    }
}
