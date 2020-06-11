using Microsoft.EntityFrameworkCore;
using GSCrmLibrary.Models.MainEntities;
using GSCrmLibrary.Models.TableModels;
using static GSCrmLibrary.Configuration.ApplicationConfig;
using System;

namespace GSCrmLibrary.Data
{
    public class MainContext : DbContext
    {
        public DbSet<Application> Applications { get; set; }
        public DbSet<ApplicationItem> ApplicationItems { get; set; }
        public DbSet<Screen> Screens { get; set; }
        public DbSet<ScreenItem> ScreenItems { get; set; }
        public DbSet<View> Views { get; set; }
        public DbSet<ViewItem> ViewItems { get; set; }
        public DbSet<Applet> Applets { get; set; }
        public DbSet<PhysicalRender> PhysicalRenders { get; set; }
        public DbSet<Control> Controls { get; set; }
        public DbSet<ControlUP> ControlUPs { get; set; }
        public DbSet<Column> Columns { get; set; }
        public DbSet<ColumnUP> ColumnUPs { get; set; }
        public DbSet<Drilldown> Drilldowns { get; set; }
        public DbSet<Icon> Icons { get; set; }
        public DbSet<BusinessObject> BusinessObjects { get; set; }
        public DbSet<BusinessObjectComponent> BusinessObjectComponents { get; set; }
        public DbSet<Link> Links { get; set; }
        public DbSet<BusinessComponent> BusinessComponents { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<Join> Joins { get; set; }
        public DbSet<JoinSpecification> JoinSpecifications { get; set; }
        public DbSet<PickMap> PickMaps { get; set; }
        public DbSet<PickList> PickLists { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<TableColumn> TableColumns { get; set; }
        public DbSet<DataEntityUp> UPs { get; set; }
        public DbSet<DirectoriesList> DirectoriesList { get; set; }
        public DbSet<DataMap> DataMaps { get; set; }
        public DbSet<DataMapObject> DataMapObjects { get; set; }
        public DbSet<DataMapObjectComponent> DataMapObjectComponents { get; set; }
        public DbSet<DataMapField> DataMapFields { get; set; }
        public MainContext() { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer($@"Server={ServerName};Database={DataBaseName};Trusted_Connection=True;");
            optionsBuilder.EnableSensitiveDataLogging();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Control>()
                .Property(a => a.ActionType)
                .HasConversion(
                    v => v.ToString(),
                    v => (ActionType)Enum.Parse(typeof(ActionType), v));
            modelBuilder
                .Entity<Column>()
                .Property(a => a.ActionType)
                .HasConversion(
                    v => v.ToString(),
                    v => (ActionType)Enum.Parse(typeof(ActionType), v));
        }
    }
}
