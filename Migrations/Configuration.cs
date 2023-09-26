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
                new Role { Type = "Member" },
                new Role { Type = "SuperAdmin" });

            context.SaveChanges();

            var adminU = new Users
            {
               // StudentNum = "00000000",
                FirstName = "Super",
                Surname = "Admin",
                Email = "govdamian@gmail.com",
                Password = "$2a$11$Gep3kcMofnXDO.5i1VInpOwgfa0fjGXTXdmmCEPVFb32Y5t3qC0ni",
                Access = true,
                Notify = true,
                StudyYear = 10,
                Qualification = "No Qualification",
                RoleId = context.Role.Single(u => u.Type == "SuperAdmin").Id,
                CreatedBy = 0
            };

            context.Users.AddOrUpdate(u => u.Email, adminU);

            context.SaveChanges();
        }
    }
}
