using System.Data.Entity.Migrations;

namespace MTechSystems.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<EntityFramework.Context.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(EntityFramework.Context.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
