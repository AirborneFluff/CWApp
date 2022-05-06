using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    public partial class AddedLinkForBOMProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "BOMs",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BOMs_ProductId",
                table: "BOMs",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_BOMs_Products_ProductId",
                table: "BOMs",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BOMs_Products_ProductId",
                table: "BOMs");

            migrationBuilder.DropIndex(
                name: "IX_BOMs_ProductId",
                table: "BOMs");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "BOMs");
        }
    }
}
