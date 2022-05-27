using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class AddedOutboundOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OutboundOrderId",
                table: "Requisitions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "OutboundOrder",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SupplierId = table.Column<int>(type: "INTEGER", nullable: false),
                    OrderNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    SupplierReference = table.Column<string>(type: "TEXT", nullable: true),
                    TaxDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboundOrder", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OutboundOrderItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OutboundOrderId = table.Column<int>(type: "INTEGER", nullable: false),
                    PartId = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<float>(type: "REAL", nullable: false),
                    UnitPrice = table.Column<float>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboundOrderItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OutboundOrderItem_OutboundOrder_OutboundOrderId",
                        column: x => x.OutboundOrderId,
                        principalTable: "OutboundOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OutboundOrderItem_Parts_PartId",
                        column: x => x.PartId,
                        principalTable: "Parts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Requisitions_OutboundOrderId",
                table: "Requisitions",
                column: "OutboundOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OutboundOrderItem_OutboundOrderId",
                table: "OutboundOrderItem",
                column: "OutboundOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OutboundOrderItem_PartId",
                table: "OutboundOrderItem",
                column: "PartId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requisitions_OutboundOrder_OutboundOrderId",
                table: "Requisitions",
                column: "OutboundOrderId",
                principalTable: "OutboundOrder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requisitions_OutboundOrder_OutboundOrderId",
                table: "Requisitions");

            migrationBuilder.DropTable(
                name: "OutboundOrderItem");

            migrationBuilder.DropTable(
                name: "OutboundOrder");

            migrationBuilder.DropIndex(
                name: "IX_Requisitions_OutboundOrderId",
                table: "Requisitions");

            migrationBuilder.DropColumn(
                name: "OutboundOrderId",
                table: "Requisitions");
        }
    }
}
