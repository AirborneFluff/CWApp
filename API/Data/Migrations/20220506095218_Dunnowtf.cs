using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    public partial class Dunnowtf : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_BOMEntry_PartId",
                table: "BOMEntry",
                column: "PartId");

            migrationBuilder.AddForeignKey(
                name: "FK_BOMEntry_Parts_PartId",
                table: "BOMEntry",
                column: "PartId",
                principalTable: "Parts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BOMEntry_Parts_PartId",
                table: "BOMEntry");

            migrationBuilder.DropIndex(
                name: "IX_BOMEntry_PartId",
                table: "BOMEntry");
        }
    }
}
