﻿// <auto-generated />
using System;
using API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace API.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20220528181329_RemovedStockUnitsRequisitions")]
    partial class RemovedStockUnitsRequisitions
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.4");

            modelBuilder.Entity("API.Entities.BOM", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<int>("ProductId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("BOMs");
                });

            modelBuilder.Entity("API.Entities.BOMEntry", b =>
                {
                    b.Property<int>("BOMId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PartId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ComponentLocation")
                        .HasColumnType("TEXT");

                    b.Property<float>("Quantity")
                        .HasColumnType("REAL");

                    b.HasKey("BOMId", "PartId");

                    b.HasIndex("PartId");

                    b.ToTable("BOMEntry");
                });

            modelBuilder.Entity("API.Entities.OutboundOrder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("OrderNumber")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SupplierId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SupplierReference")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("TaxDate")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("SupplierId");

                    b.ToTable("OutboundOrder");
                });

            modelBuilder.Entity("API.Entities.OutboundOrderItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("OutboundOrderId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PartId")
                        .HasColumnType("INTEGER");

                    b.Property<float>("Quantity")
                        .HasColumnType("REAL");

                    b.Property<float>("UnitPrice")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.HasIndex("OutboundOrderId");

                    b.HasIndex("PartId");

                    b.ToTable("OutboundOrderItem");
                });

            modelBuilder.Entity("API.Entities.Part", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<float>("BufferValue")
                        .HasColumnType("REAL");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Notes")
                        .HasColumnType("TEXT");

                    b.Property<string>("PartCode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("StockUnits")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("PartCode")
                        .IsUnique();

                    b.ToTable("Parts");
                });

            modelBuilder.Entity("API.Entities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedName")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique();

                    b.ToTable("Products");
                });

            modelBuilder.Entity("API.Entities.Requisition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<bool>("ForBuffer")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("OutboundOrderId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PartId")
                        .HasColumnType("INTEGER");

                    b.Property<float>("Quantity")
                        .HasColumnType("REAL");

                    b.Property<float>("StockRemaining")
                        .HasColumnType("REAL");

                    b.Property<bool>("Urgent")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("OutboundOrderId");

                    b.HasIndex("PartId");

                    b.HasIndex("UserId");

                    b.ToTable("Requisitions");
                });

            modelBuilder.Entity("API.Entities.SourcePrice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<float>("Quantity")
                        .HasColumnType("REAL");

                    b.Property<int>("SupplySourceId")
                        .HasColumnType("INTEGER");

                    b.Property<float>("UnitPrice")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.HasIndex("SupplySourceId");

                    b.ToTable("SourcePrices");
                });

            modelBuilder.Entity("API.Entities.StockLevelEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<int>("PartId")
                        .HasColumnType("INTEGER");

                    b.Property<float>("RemainingStock")
                        .HasColumnType("REAL");

                    b.Property<string>("StockUnits")
                        .HasColumnType("TEXT");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("PartId");

                    b.HasIndex("UserId");

                    b.ToTable("StockLevelEntries");
                });

            modelBuilder.Entity("API.Entities.Supplier", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedName")
                        .HasColumnType("TEXT");

                    b.Property<string>("Website")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique();

                    b.ToTable("Suppliers");
                });

            modelBuilder.Entity("API.Entities.SupplySource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ManufacturerSKU")
                        .HasColumnType("TEXT");

                    b.Property<float>("MinimumOrderQuantity")
                        .HasColumnType("REAL");

                    b.Property<string>("Notes")
                        .HasColumnType("TEXT");

                    b.Property<float>("PackSize")
                        .HasColumnType("REAL");

                    b.Property<int>("PartId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("RoHS")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SupplierId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SupplierSKU")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("PartId");

                    b.HasIndex("SupplierId");

                    b.ToTable("SupplySources");
                });

            modelBuilder.Entity("API.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("FirstName")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("API.Entities.BOM", b =>
                {
                    b.HasOne("API.Entities.Product", null)
                        .WithMany("BOMs")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("API.Entities.BOMEntry", b =>
                {
                    b.HasOne("API.Entities.BOM", null)
                        .WithMany("Parts")
                        .HasForeignKey("BOMId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Entities.Part", "Part")
                        .WithMany()
                        .HasForeignKey("PartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Part");
                });

            modelBuilder.Entity("API.Entities.OutboundOrder", b =>
                {
                    b.HasOne("API.Entities.Supplier", "Supplier")
                        .WithMany()
                        .HasForeignKey("SupplierId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Supplier");
                });

            modelBuilder.Entity("API.Entities.OutboundOrderItem", b =>
                {
                    b.HasOne("API.Entities.OutboundOrder", null)
                        .WithMany("Items")
                        .HasForeignKey("OutboundOrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Entities.Part", "Part")
                        .WithMany("Orders")
                        .HasForeignKey("PartId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Part");
                });

            modelBuilder.Entity("API.Entities.Requisition", b =>
                {
                    b.HasOne("API.Entities.OutboundOrder", "OutboundOrder")
                        .WithMany()
                        .HasForeignKey("OutboundOrderId");

                    b.HasOne("API.Entities.Part", "Part")
                        .WithMany()
                        .HasForeignKey("PartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OutboundOrder");

                    b.Navigation("Part");

                    b.Navigation("User");
                });

            modelBuilder.Entity("API.Entities.SourcePrice", b =>
                {
                    b.HasOne("API.Entities.SupplySource", null)
                        .WithMany("Prices")
                        .HasForeignKey("SupplySourceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("API.Entities.StockLevelEntry", b =>
                {
                    b.HasOne("API.Entities.Part", "Part")
                        .WithMany()
                        .HasForeignKey("PartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Part");

                    b.Navigation("User");
                });

            modelBuilder.Entity("API.Entities.SupplySource", b =>
                {
                    b.HasOne("API.Entities.Part", null)
                        .WithMany("SupplySources")
                        .HasForeignKey("PartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Entities.Supplier", "Supplier")
                        .WithMany()
                        .HasForeignKey("SupplierId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Supplier");
                });

            modelBuilder.Entity("API.Entities.BOM", b =>
                {
                    b.Navigation("Parts");
                });

            modelBuilder.Entity("API.Entities.OutboundOrder", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("API.Entities.Part", b =>
                {
                    b.Navigation("Orders");

                    b.Navigation("SupplySources");
                });

            modelBuilder.Entity("API.Entities.Product", b =>
                {
                    b.Navigation("BOMs");
                });

            modelBuilder.Entity("API.Entities.SupplySource", b =>
                {
                    b.Navigation("Prices");
                });
#pragma warning restore 612, 618
        }
    }
}
