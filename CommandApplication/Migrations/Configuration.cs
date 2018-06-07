namespace CommandApplication.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CommandApplication.DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(CommandApplication.DataContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            context.ConnectionAddresses.AddOrUpdate(
                ca => ca.Address,
                new ConnectionAddress { Id = 1, Address = "10.0.0.63", Comment = "Nettverk hjemme", isActive = true }
                );
        }
    }
}
