using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class RemovedRemaingStockFromDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StockRemaining",
                table: "Requisitions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "StockRemaining",
                table: "Requisitions",
                type: "REAL",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
