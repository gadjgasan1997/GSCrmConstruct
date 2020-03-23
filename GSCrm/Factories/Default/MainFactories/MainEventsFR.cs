using GSCrm.Data;
using Microsoft.EntityFrameworkCore;

namespace GSCrm.Factories.Default.MainFactories
{
    public class MainEventsFR<MainTable, MainBusinessComponent>
        where MainTable : Models.Default.MainEntities.MainTable, new()
        where MainBusinessComponent : Models.Default.MainEntities.MainBusinessComponent, new()
    {

    }
}