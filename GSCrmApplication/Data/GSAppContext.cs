using GSCrmApplication.Models.TableModels;
using GSCrmLibrary.Data;
using Microsoft.EntityFrameworkCore;

namespace GSCrmApplication.Data
{
    public class GSAppContext : MainContext
    {
        public DbSet<Position> Position { get; set; }
        public DbSet<Division> Division { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Contact> Contact { get; set; }
        public DbSet<Invoice> Invoice { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Account> Account { get; set; }
    }
}