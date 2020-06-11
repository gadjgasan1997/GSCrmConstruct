using System.IO;
using Microsoft.EntityFrameworkCore;
using GSCrmLibrary.CodeGeneration;
using GSCrmLibrary.Configuration;
using GSCrmLibrary.Data;
using GSCrmLibrary.Factories.MainFactories;
using GSCrmLibrary.Models.BusinessComponentModels;
using GSCrmLibrary.Models.TableModels;
using static GSCrmLibrary.Utils;

namespace GSCrmLibrary.Factories.DataBUSFactories
{
    public class DataBUSScreenFR<TContext> : DataBUSFactory<Screen, BUSScreen, TContext>
        where TContext : MainContext, new()
    {
        public override BUSScreen DataToBusiness(Screen dataEntity, TContext context)
        {
            BUSScreen businessEntity = base.DataToBusiness(dataEntity, context);
            businessEntity.Header = dataEntity.Header;
            return businessEntity;
        }
        public override Screen BusinessToData(Screen screen, BUSScreen businessEntity, TContext context, bool NewRecord)
        {
            Screen dataEntity = base.BusinessToData(screen, businessEntity, context, NewRecord);
            dataEntity.Header = businessEntity.Header;
            dataEntity.Routing = $"/{dataEntity.Name}/";
            return dataEntity;
        }
        public override void OnRecordDelete(Screen recordToDelete, DbSet<Screen> entities, TContext context)
        {
            string permissibleName = GetPermissibleName(recordToDelete.Name) + "Controller";
            CodGenUtils.DeleteEntityFile(permissibleName, "ScreenController");
            string path = $"{EntitiesConfig.GetEntityFullPath(ApplicationConfig.GSCrmApplication, permissibleName, "ViewCshtml")}\\Index.cshtml";
            if (File.Exists(path))
                File.Delete(path);
            if (Directory.Exists(path.Replace("\\Index.cshtml", "")))
                Directory.Delete(path.Replace("\\Index.cshtml", ""));
            base.OnRecordDelete(recordToDelete, entities, context);
        }
    }
}
