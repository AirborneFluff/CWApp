using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class StockLevels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StockLevelEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PartId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    RemainingStock = table.Column<float>(type: "REAL", nullable: false),
                    StockUnits = table.Column<string>(type: "TEXT", nullable: true),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockLevelEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockLevelEntries_Parts_PartId",
                        column: x => x.PartId,
                        principalTable: "Parts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockLevelEntries_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_OutboundOrder_SupplierId",
                table: "OutboundOrder",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_StockLevelEntries_PartId",
                table: "StockLevelEntries",
                column: "PartId");

            migrationBuilder.CreateIndex(
                name: "IX_StockLevelEntries_UserId",
                table: "StockLevelEntries",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_OutboundOrder_Suppliers_SupplierId",
                table: "OutboundOrder",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OutboundOrder_Suppliers_SupplierId",
                table: "OutboundOrder");

            migrationBuilder.DropTable(
                name: "StockLevelEntries");

            migrationBuilder.DropIndex(
                name: "IX_OutboundOrder_SupplierId",
                table: "OutboundOrder");
        }
    }
}
