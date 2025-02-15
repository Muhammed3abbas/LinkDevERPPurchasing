﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Purchasing.Infrastructure.Data;

#nullable disable

namespace Purchasing.Infrastructure.Migrations
{
    [DbContext(typeof(PurchasingDbContext))]
    [Migration("20250107112708_Update-POItem")]
    partial class UpdatePOItem
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Purchasing.Domain.Models.PurchaseOrder", b =>
                {
                    b.Property<string>("POnumber")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("ActivationState")
                        .HasColumnType("int");

                    b.Property<DateTime>("IssuedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("POnumber");

                    b.ToTable("PurchaseOrders");
                });

            modelBuilder.Entity("Purchasing.Domain.Models.PurchaseOrderItem", b =>
                {
                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("PurchaseOrderPOnumber")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int?>("SerialNumber")
                        .HasColumnType("int");

                    b.HasKey("Code");

                    b.HasIndex("PurchaseOrderPOnumber");

                    b.ToTable("PurchaseOrderItems");
                });

            modelBuilder.Entity("Purchasing.Domain.Models.PurchaseOrderItem", b =>
                {
                    b.HasOne("Purchasing.Domain.Models.PurchaseOrder", null)
                        .WithMany("Items")
                        .HasForeignKey("PurchaseOrderPOnumber");
                });

            modelBuilder.Entity("Purchasing.Domain.Models.PurchaseOrder", b =>
                {
                    b.Navigation("Items");
                });
#pragma warning restore 612, 618
        }
    }
}
