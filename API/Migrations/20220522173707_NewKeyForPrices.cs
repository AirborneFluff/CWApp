using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class NewKeyForPrices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SourcePrices",
                table: "SourcePrices");

            migrationBuilder.DropIndex(
                name: "IX_SourcePrices_SupplySourceId",
                table: "SourcePrices");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SourcePrices",
                table: "SourcePrices",
                columns: new[] { "SupplySourceId", "Quantity", "UnitPrice" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SourcePrices",
                table: "SourcePrices");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SourcePrices",
                table: "SourcePrices",
                columns: new[] { "Quantity", "UnitPrice" });

            migrationBuilder.CreateIndex(
                name: "IX_SourcePrices_SupplySourceId",
                table: "SourcePrices",
                column: "SupplySourceId");
        }
    }
}
