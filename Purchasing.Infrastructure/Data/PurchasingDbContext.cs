using Microsoft.EntityFrameworkCore;
using Purchasing.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchasing.Infrastructure.Data
{
    public class PurchasingDbContext : DbContext
    {
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }

        public PurchasingDbContext(DbContextOptions<PurchasingDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PurchaseOrder>().HasKey(po => po.POnumber);
            modelBuilder.Entity<PurchaseOrderItem>().HasKey(poi => poi.Code);
            // Additional configurations
        }
    }
}
