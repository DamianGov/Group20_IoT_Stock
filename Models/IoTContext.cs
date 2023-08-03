using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Group20_IoT.Models
{
    public class IoTContext : DbContext
    {
        public IoTContext() : base("IoTDb")
        { 
        
        }

        public DbSet<Role> Role { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Room> Room { get; set; }   
        public DbSet<StorageArea> StorageArea { get; set; }    
        public DbSet<Stock> Stock { get; set; }

    }
}