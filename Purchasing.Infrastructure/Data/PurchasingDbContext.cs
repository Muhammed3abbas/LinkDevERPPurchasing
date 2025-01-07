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
            // Configure PurchaseOrder
            modelBuilder.Entity<PurchaseOrder>().HasKey(po => po.POnumber);

            // Configure PurchaseOrderItem
            modelBuilder.Entity<PurchaseOrderItem>().HasKey(poi => poi.Code);

            // Configure the junction table
            modelBuilder.Entity<PurchaseOrderItemMapping>()
                .HasKey(mapping => mapping.SerialNumber);

            modelBuilder.Entity<PurchaseOrderItemMapping>()
                .HasOne(mapping => mapping.PurchaseOrder)
                .WithMany(po => po.PurchaseOrderItemMappings)
                .HasForeignKey(mapping => mapping.PurchaseOrderPOnumber);

            modelBuilder.Entity<PurchaseOrderItemMapping>()
                .HasOne(mapping => mapping.PurchaseOrderItem)
                .WithMany(poi => poi.PurchaseOrderItemMappings)
                .HasForeignKey(mapping => mapping.PurchaseOrderItemCode);

        }
    }
}
