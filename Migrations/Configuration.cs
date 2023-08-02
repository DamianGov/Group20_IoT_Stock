namespace Group20_IoT.Migrations
{
    using Group20_IoT.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Group20_IoT.Models.IoTContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(IoTContext context)
        {
            context.UserType.AddOrUpdate(u => u.Type,
                new UserType { Type = "Admin" },
                new UserType { Type = "Standard" });

            context.SaveChanges();

            var adminU = new Users
            {
                Name = "Damian",
                Email = "govdamian@gmail.com",
                UserTypeId = context.UserType.Single(u => u.Type == "Admin").Id
            };

            context.Users.AddOrUpdate(u => u.Email, adminU);

            context.SaveChanges();
        }
    }
}
