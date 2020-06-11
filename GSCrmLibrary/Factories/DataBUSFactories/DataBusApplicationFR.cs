using GSCrmLibrary.Data;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Models.BusinessComponentModels;
using GSCrmLibrary.Factories.MainFactories;

namespace GSCrmLibrary.Factories.DataBUSFactories
{
    public class DataBUSApplicationFR<TContext> : DataBUSFactory<Application, BUSApplication, TContext>
        where TContext : MainContext, new()
    { }
}
