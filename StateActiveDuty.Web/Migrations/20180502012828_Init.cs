using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StateActiveDuty.Web.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    POC_Name = table.Column<string>(nullable: true),
                    POC_PhoneNumber = table.Column<string>(nullable: true),
                    POC_Role = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateTime>(nullable: false),
                    UnitId = table.Column<int>(nullable: false),
                    Vendor_Name = table.Column<string>(nullable: true),
                    Vendor_BusinessPhone = table.Column<string>(nullable: true),
                    Vendor_POC_Name = table.Column<string>(nullable: true),
                    Vendor_POC_PhoneNumber = table.Column<string>(nullable: true),
                    Vendor_POC_Role = table.Column<string>(nullable: true),
                    Vendor_PhysicalAddress_Line1 = table.Column<string>(nullable: true),
                    Vendor_PhysicalAddress_City = table.Column<string>(nullable: true),
                    Vendor_PhysicalAddress_State = table.Column<string>(nullable: true),
                    Vendor_PhysicalAddress_ZipCode = table.Column<string>(nullable: true),
                    Vendor_RemitToAddress_Line1 = table.Column<string>(nullable: true),
                    Vendor_RemitToAddress_City = table.Column<string>(nullable: true),
                    Vendor_RemitToAddress_State = table.Column<string>(nullable: true),
                    Vendor_RemitToAddress_ZipCode = table.Column<string>(nullable: true),
                    Priority = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderEvent",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Timestamp = table.Column<DateTimeOffset>(nullable: false),
                    Status = table.Column<byte>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    Comments = table.Column<string>(nullable: true),
                    PurchaseOrderId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderEvent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderEvent_PurchaseOrders_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalTable: "PurchaseOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderEvent_PurchaseOrderId",
                table: "OrderEvent",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_UnitId",
                table: "PurchaseOrders",
                column: "UnitId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderEvent");

            migrationBuilder.DropTable(
                name: "PurchaseOrders");

            migrationBuilder.DropTable(
                name: "Units");
        }
    }
}
