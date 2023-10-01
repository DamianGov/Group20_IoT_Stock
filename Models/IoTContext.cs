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
        public DbSet<UserLoginTracking> UserLoginTracking { get; set; } 
        public DbSet<RequestStock> RequestStock { get; set; }
        public DbSet<StockDiscrepancy> StockDiscrepancy { get; set; }
        public DbSet<RequestLoanStock> RequestLoanStock { get; set; }
        public DbSet<LoanStatus> LoanStatus { get; set; }

        public DbSet<ExtensionRequest> ExtensionRequest { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RequestStock>()
                .HasRequired(r => r.Users)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RequestLoanStock>()
                .HasRequired(r => r.Users)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LoanStatus>()
        .HasRequired(ls => ls.Users)
        .WithMany()
        .HasForeignKey(ls => ls.AccBy)
        .WillCascadeOnDelete(false);

            modelBuilder.Entity<Stock>()
        .HasRequired(ls => ls.Users)
        .WithMany()
        .HasForeignKey(ls => ls.CreatedBy)
        .WillCascadeOnDelete(false);


            base.OnModelCreating(modelBuilder);
        }
    }
}