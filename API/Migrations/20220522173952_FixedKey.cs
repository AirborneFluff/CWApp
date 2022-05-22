using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class FixedKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SourcePrices",
                table: "SourcePrices");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "SourcePrices",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SourcePrices",
                table: "SourcePrices",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_SourcePrices_SupplySourceId",
                table: "SourcePrices",
                column: "SupplySourceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SourcePrices",
                table: "SourcePrices");

            migrationBuilder.DropIndex(
                name: "IX_SourcePrices_SupplySourceId",
                table: "SourcePrices");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "SourcePrices");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SourcePrices",
                table: "SourcePrices",
                columns: new[] { "SupplySourceId", "Quantity", "UnitPrice" });
        }
    }
}
