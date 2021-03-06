﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Migrations;
using StateActiveDuty.Web.Models;

namespace StateActiveDuty.Web.Migrations
{
    [DbContext(typeof(Database))]
    partial class DatabaseModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.0-preview2-30571");

            modelBuilder.Entity("StateActiveDuty.Web.Models.PurchaseOrder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte>("Category");

                    b.Property<DateTime>("Date");

                    b.Property<string>("Identifier");

                    b.Property<string>("Justification");

                    b.Property<byte>("Meal");

                    b.Property<string>("Operation");

                    b.Property<byte>("Priority");

                    b.Property<int>("UnitId");

                    b.HasKey("Id");

                    b.HasIndex("UnitId");

                    b.ToTable("PurchaseOrders");
                });

            modelBuilder.Entity("StateActiveDuty.Web.Models.PurchaseOrder+OrderEvent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Comments");

                    b.Property<int?>("PurchaseOrderId");

                    b.Property<byte>("Status");

                    b.Property<DateTimeOffset>("Timestamp");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.HasIndex("PurchaseOrderId");

                    b.ToTable("OrderEvent");
                });

            modelBuilder.Entity("StateActiveDuty.Web.Models.Unit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CommandOrTaskForce");

                    b.Property<string>("Name");

                    b.Property<string>("Phone");

                    b.HasKey("Id");

                    b.ToTable("Units");
                });

            modelBuilder.Entity("StateActiveDuty.Web.Models.PurchaseOrder", b =>
                {
                    b.HasOne("StateActiveDuty.Web.Models.Unit", "Unit")
                        .WithMany()
                        .HasForeignKey("UnitId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.OwnsOne("StateActiveDuty.Web.Models.PurchaseOrder+OrderVendor", "Vendor", b1 =>
                        {
                            b1.Property<int?>("PurchaseOrderId");

                            b1.Property<string>("BusinessPhone");

                            b1.Property<string>("FedID");

                            b1.Property<string>("Name");

                            b1.ToTable("PurchaseOrders");

                            b1.HasOne("StateActiveDuty.Web.Models.PurchaseOrder")
                                .WithOne("Vendor")
                                .HasForeignKey("StateActiveDuty.Web.Models.PurchaseOrder+OrderVendor", "PurchaseOrderId")
                                .OnDelete(DeleteBehavior.Cascade);

                            b1.OwnsOne("StateActiveDuty.Web.Models.PointOfContact", "POC", b2 =>
                                {
                                    b2.Property<int?>("OrderVendorPurchaseOrderId");

                                    b2.Property<string>("Name");

                                    b2.Property<string>("PhoneNumber");

                                    b2.Property<string>("Role");

                                    b2.ToTable("PurchaseOrders");

                                    b2.HasOne("StateActiveDuty.Web.Models.PurchaseOrder+OrderVendor")
                                        .WithOne("POC")
                                        .HasForeignKey("StateActiveDuty.Web.Models.PointOfContact", "OrderVendorPurchaseOrderId")
                                        .OnDelete(DeleteBehavior.Cascade);
                                });

                            b1.OwnsOne("StateActiveDuty.Web.Models.PurchaseOrder+Address", "PhysicalAddress", b2 =>
                                {
                                    b2.Property<int?>("OrderVendorPurchaseOrderId");

                                    b2.Property<string>("City");

                                    b2.Property<string>("Line1");

                                    b2.Property<string>("State");

                                    b2.Property<string>("ZipCode");

                                    b2.ToTable("PurchaseOrders");

                                    b2.HasOne("StateActiveDuty.Web.Models.PurchaseOrder+OrderVendor")
                                        .WithOne("PhysicalAddress")
                                        .HasForeignKey("StateActiveDuty.Web.Models.PurchaseOrder+Address", "OrderVendorPurchaseOrderId")
                                        .OnDelete(DeleteBehavior.Cascade);
                                });

                            b1.OwnsOne("StateActiveDuty.Web.Models.PurchaseOrder+Address", "RemitToAddress", b2 =>
                                {
                                    b2.Property<int?>("OrderVendorPurchaseOrderId");

                                    b2.Property<string>("City");

                                    b2.Property<string>("Line1");

                                    b2.Property<string>("State");

                                    b2.Property<string>("ZipCode");

                                    b2.ToTable("PurchaseOrders");

                                    b2.HasOne("StateActiveDuty.Web.Models.PurchaseOrder+OrderVendor")
                                        .WithOne("RemitToAddress")
                                        .HasForeignKey("StateActiveDuty.Web.Models.PurchaseOrder+Address", "OrderVendorPurchaseOrderId")
                                        .OnDelete(DeleteBehavior.Cascade);
                                });
                        });
                });

            modelBuilder.Entity("StateActiveDuty.Web.Models.PurchaseOrder+OrderEvent", b =>
                {
                    b.HasOne("StateActiveDuty.Web.Models.PurchaseOrder")
                        .WithMany("Events")
                        .HasForeignKey("PurchaseOrderId");
                });

            modelBuilder.Entity("StateActiveDuty.Web.Models.Unit", b =>
                {
                    b.OwnsOne("StateActiveDuty.Web.Models.PointOfContact", "POC", b1 =>
                        {
                            b1.Property<int>("UnitId");

                            b1.Property<string>("Name");

                            b1.Property<string>("PhoneNumber");

                            b1.Property<string>("Role");

                            b1.ToTable("Units");

                            b1.HasOne("StateActiveDuty.Web.Models.Unit")
                                .WithOne("POC")
                                .HasForeignKey("StateActiveDuty.Web.Models.PointOfContact", "UnitId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
