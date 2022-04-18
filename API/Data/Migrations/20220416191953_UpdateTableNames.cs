using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    public partial class UpdateTableNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SourcePrice_SupplySource_SupplySourceId",
                table: "SourcePrice");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplySource_Parts_PartId",
                table: "SupplySource");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplySource_Suppliers_SupplierId",
                table: "SupplySource");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SupplySource",
                table: "SupplySource");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SourcePrice",
                table: "SourcePrice");

            migrationBuilder.RenameTable(
                name: "SupplySource",
                newName: "SupplySources");

            migrationBuilder.RenameTable(
                name: "SourcePrice",
                newName: "SourcePrices");

            migrationBuilder.RenameIndex(
                name: "IX_SupplySource_SupplierId",
                table: "SupplySources",
                newName: "IX_SupplySources_SupplierId");

            migrationBuilder.RenameIndex(
                name: "IX_SupplySource_PartId",
                table: "SupplySources",
                newName: "IX_SupplySources_PartId");

            migrationBuilder.RenameIndex(
                name: "IX_SourcePrice_SupplySourceId",
                table: "SourcePrices",
                newName: "IX_SourcePrices_SupplySourceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SupplySources",
                table: "SupplySources",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SourcePrices",
                table: "SourcePrices",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SourcePrices_SupplySources_SupplySourceId",
                table: "SourcePrices",
                column: "SupplySourceId",
                principalTable: "SupplySources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SupplySources_Parts_PartId",
                table: "SupplySources",
                column: "PartId",
                principalTable: "Parts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SupplySources_Suppliers_SupplierId",
                table: "SupplySources",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SourcePrices_SupplySources_SupplySourceId",
                table: "SourcePrices");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplySources_Parts_PartId",
                table: "SupplySources");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplySources_Suppliers_SupplierId",
                table: "SupplySources");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SupplySources",
                table: "SupplySources");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SourcePrices",
                table: "SourcePrices");

            migrationBuilder.RenameTable(
                name: "SupplySources",
                newName: "SupplySource");

            migrationBuilder.RenameTable(
                name: "SourcePrices",
                newName: "SourcePrice");

            migrationBuilder.RenameIndex(
                name: "IX_SupplySources_SupplierId",
                table: "SupplySource",
                newName: "IX_SupplySource_SupplierId");

            migrationBuilder.RenameIndex(
                name: "IX_SupplySources_PartId",
                table: "SupplySource",
                newName: "IX_SupplySource_PartId");

            migrationBuilder.RenameIndex(
                name: "IX_SourcePrices_SupplySourceId",
                table: "SourcePrice",
                newName: "IX_SourcePrice_SupplySourceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SupplySource",
                table: "SupplySource",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SourcePrice",
                table: "SourcePrice",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SourcePrice_SupplySource_SupplySourceId",
                table: "SourcePrice",
                column: "SupplySourceId",
                principalTable: "SupplySource",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SupplySource_Parts_PartId",
                table: "SupplySource",
                column: "PartId",
                principalTable: "Parts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SupplySource_Suppliers_SupplierId",
                table: "SupplySource",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
