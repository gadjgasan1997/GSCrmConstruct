using GSCrmLibrary.Models.BusinessComponentModels;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Factories.MainFactories;
using GSCrmLibrary.Data;

namespace GSCrmLibrary.Factories.DataBUSFactories
{
    public class DataBUSPRFR<TContext> : DataBUSFactory<PhysicalRender, BUSPhysicalRender, TContext>
        where TContext : MainContext, new()
    { }
}
