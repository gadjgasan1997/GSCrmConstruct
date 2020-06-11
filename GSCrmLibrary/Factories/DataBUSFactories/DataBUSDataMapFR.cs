using GSCrmLibrary.Data;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Models.BusinessComponentModels;
using GSCrmLibrary.Factories.MainFactories;

namespace GSCrmLibrary.Factories.DataBUSFactories
{
    public class DataBUSDataMapFR<TContext> : DataBUSFactory<DataMap, BUSDataMap, TContext>
        where TContext : MainContext, new()
    { }
}
