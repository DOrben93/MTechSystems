using MTechSystems.EntityFramework.Models;
using System.Data.Entity;
using System.Threading.Tasks;

namespace MTechSystems.EntityFramework.Context
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        
        public ApplicationDbContext() : base("Conexion")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Migrations.Configuration>());
        }


        protected override void OnModelCreating(DbModelBuilder dbModelBuilder)
        {
            //Here we would set any rules regarding the behaviour and relations of the tables and their columns
        }

        public override Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
    }
}
