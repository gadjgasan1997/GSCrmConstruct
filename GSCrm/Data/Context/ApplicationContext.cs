using Microsoft.EntityFrameworkCore;
using GSCrm.Models.Default.TableModels;
using GSCrm.Models.Default.MainEntities;

namespace GSCrm.Data
{
    public class ApplicationContext : DbContext
    {
        #region UILayer
        public DbSet<Application> Applications { get; set; }
        public DbSet<Screen> Screens { get; set; }
        public DbSet<ScreenItem> ScreenItems { get; set; }
        public DbSet<View> Views { get; set; }
        public DbSet<ViewItem> ViewItems { get; set; }
        public DbSet<Applet> Applets { get; set; }
        public DbSet<PR> PhysicalRenders { get; set; }
        public DbSet<Control> Controls { get; set; }
        public DbSet<ControlUP> ControlUPs { get; set; }
        public DbSet<Column> Columns { get; set; }
        public DbSet<Icon> Icons { get; set; }
        public DbSet<Action> Actions { get; set; }
        public DbSet<ActionUP> ActionUPs { get; set; }
        #endregion

        #region Business Object Layer
        public DbSet<BusObject> BusinessObjects { get; set; }
        public DbSet<BOComponent> BusinessObjectComponents { get; set; }
        public DbSet<Link> Links { get; set; }
        public DbSet<BusComp> BusinessComponents { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<Join> Joins { get; set; }
        public DbSet<JoinSpecification> JoinSpecifications { get; set; }
        public DbSet<PL> PickLists { get; set; }
        #endregion

        #region Data Object Layer
        public DbSet<Table> Tables { get; set; }
        public DbSet<TableColumn> TableColumns { get; set; }
        #endregion

        #region Common
        public DbSet<MainTableUP> UPs { get; set; }
        
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options) { }
        #endregion
    }
}
