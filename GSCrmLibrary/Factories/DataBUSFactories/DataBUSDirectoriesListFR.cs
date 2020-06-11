using GSCrmLibrary.Data;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Models.BusinessComponentModels;
using GSCrmLibrary.Factories.MainFactories;

namespace GSCrmLibrary.Factories.DataBUSFactories
{
    public class DataBUSDirectoriesListFR<TContext> : DataBUSFactory<DirectoriesList, BUSDirectoriesList, TContext>
        where TContext : MainContext, new()
    {
        public override BUSDirectoriesList DataToBusiness(DirectoriesList dataEntity, TContext context)
        {
            BUSDirectoriesList businessEntity = base.DataToBusiness(dataEntity, context);
            businessEntity.DirectoryType = dataEntity.DirectoryType;
            businessEntity.DisplayValue = dataEntity.DisplayValue;
            businessEntity.Language = dataEntity.Language;
            businessEntity.LIC = dataEntity.LIC;
            return businessEntity;
        }
        public override DirectoriesList BusinessToData(DirectoriesList directoriesList, BUSDirectoriesList businessEntity, TContext context, bool NewRecord)
        {
            DirectoriesList dataEntity = base.BusinessToData(directoriesList, businessEntity, context, NewRecord);
            dataEntity.DirectoryType = businessEntity.DirectoryType;
            dataEntity.DisplayValue = businessEntity.DisplayValue;
            dataEntity.Language = businessEntity.Language;
            dataEntity.LIC = businessEntity.LIC;
            return dataEntity;
        }
    }
}
