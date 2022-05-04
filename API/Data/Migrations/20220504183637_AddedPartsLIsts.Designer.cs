﻿// <auto-generated />
using System;
using API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace API.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20220504183637_AddedPartsLIsts")]
    partial class AddedPartsLIsts
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.4");

            modelBuilder.Entity("API.Entities.Part", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("BufferUnit")
                        .HasColumnType("TEXT");

                    b.Property<float>("BufferValue")
                        .HasColumnType("REAL");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Notes")
                        .HasColumnType("TEXT");

                    b.Property<string>("PartCode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("PartCode")
                        .IsUnique();

                    b.ToTable("Parts");
                });

            modelBuilder.Entity("API.Entities.PartsList", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("PartsLists");
                });

            modelBuilder.Entity("API.Entities.PartsListEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ComponentLocation")
                        .HasColumnType("TEXT");

                    b.Property<int>("PartId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PartsListId")
                        .HasColumnType("INTEGER");

                    b.Property<float>("Quantity")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.HasIndex("PartId");

                    b.HasIndex("PartsListId");

                    b.ToTable("PartsListEntry");
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

            modelBuilder.Entity("API.Entities.PartsListEntry", b =>
                {
                    b.HasOne("API.Entities.Part", "Part")
                        .WithMany()
                        .HasForeignKey("PartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Entities.PartsList", "PartsList")
                        .WithMany("Parts")
                        .HasForeignKey("PartsListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Part");

                    b.Navigation("PartsList");
                });

            modelBuilder.Entity("API.Entities.SourcePrice", b =>
                {
                    b.HasOne("API.Entities.SupplySource", null)
                        .WithMany("Prices")
                        .HasForeignKey("SupplySourceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
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

            modelBuilder.Entity("API.Entities.Part", b =>
                {
                    b.Navigation("SupplySources");
                });

            modelBuilder.Entity("API.Entities.PartsList", b =>
                {
                    b.Navigation("Parts");
                });

            modelBuilder.Entity("API.Entities.SupplySource", b =>
                {
                    b.Navigation("Prices");
                });
#pragma warning restore 612, 618
        }
    }
}
