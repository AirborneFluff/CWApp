using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    public partial class NonNullableSourceId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SourcePrices_SupplySources_SupplySourceId",
                table: "SourcePrices");

            migrationBuilder.AlterColumn<int>(
                name: "SupplySourceId",
                table: "SourcePrices",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SourcePrices_SupplySources_SupplySourceId",
                table: "SourcePrices",
                column: "SupplySourceId",
                principalTable: "SupplySources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SourcePrices_SupplySources_SupplySourceId",
                table: "SourcePrices");

            migrationBuilder.AlterColumn<int>(
                name: "SupplySourceId",
                table: "SourcePrices",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_SourcePrices_SupplySources_SupplySourceId",
                table: "SourcePrices",
                column: "SupplySourceId",
                principalTable: "SupplySources",
                principalColumn: "Id");
        }
    }
}
