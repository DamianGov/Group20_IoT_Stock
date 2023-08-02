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

        public DbSet<UserType> UserType { get; set; }
        public DbSet<Users> Users { get; set; }


    }
}