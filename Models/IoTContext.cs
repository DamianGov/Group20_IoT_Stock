using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Group20_IoT.Models
{
    public class IoTContext : DbContext
    {
        public IoTContext() : base("IoT_Db")
        {
            Database.SetInitializer<IoTContext>(null);
            //Database.SetInitializer(new CreateDatabaseIfNotExists<IoTContext>());
        }

        public static void CreateDatabaseIfNotExists()
        {
            using (var context = new IoTContext())
            {
                // Create the database explicitly without accessing any tables
                context.Database.Create();
            }
        }

        public DbSet<Role> Role { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Room> Room { get; set; }   
        public DbSet<StorageArea> StorageArea { get; set; }    
        public DbSet<Stock> Stock { get; set; }

    }
}