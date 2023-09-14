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

        protected override void Seed(Group20_IoT.Models.IoTContext context)
        {
            context.Role.AddOrUpdate(u => u.Type,
                new Role { Type = "Admin" },
                new Role { Type = "Standard" },
                new Role { Type = "SuperUser" });

            context.SaveChanges();

            var adminU = new Users
            {
                Name = "Super User",
                Email = "govdamian@gmail.com",
                Password = "$2a$11$whipL/4TlRNmEYIm0T0Ha.3eXZjstkTj8uI79/omgrIAybqmTHkYa",
                Access = true,
                StudyYear = 10,
                Qualification = "No Qualification",
                RoleId = context.Role.Single(u => u.Type == "SuperUser").Id,
                CreatedBy = 0
            };

            context.Users.AddOrUpdate(u => u.Email, adminU);

            context.SaveChanges();
        }
    }
}
