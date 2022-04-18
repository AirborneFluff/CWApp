using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    public partial class PartIDNonNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupplySources_Parts_PartId",
                table: "SupplySources");

            migrationBuilder.AlterColumn<int>(
                name: "PartId",
                table: "SupplySources",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SupplySources_Parts_PartId",
                table: "SupplySources",
                column: "PartId",
                principalTable: "Parts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupplySources_Parts_PartId",
                table: "SupplySources");

            migrationBuilder.AlterColumn<int>(
                name: "PartId",
                table: "SupplySources",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_SupplySources_Parts_PartId",
                table: "SupplySources",
                column: "PartId",
                principalTable: "Parts",
                principalColumn: "Id");
        }
    }
}
