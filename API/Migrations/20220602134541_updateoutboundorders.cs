using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class updateoutboundorders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OutboundOrder_Suppliers_SupplierId",
                table: "OutboundOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_OutboundOrderItem_OutboundOrder_OutboundOrderId",
                table: "OutboundOrderItem");

            migrationBuilder.DropForeignKey(
                name: "FK_Requisitions_OutboundOrder_OutboundOrderId",
                table: "Requisitions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OutboundOrder",
                table: "OutboundOrder");

            migrationBuilder.RenameTable(
                name: "OutboundOrder",
                newName: "OutboundOrders");

            migrationBuilder.RenameIndex(
                name: "IX_OutboundOrder_SupplierId",
                table: "OutboundOrders",
                newName: "IX_OutboundOrders_SupplierId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OutboundOrders",
                table: "OutboundOrders",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OutboundOrderItem_OutboundOrders_OutboundOrderId",
                table: "OutboundOrderItem",
                column: "OutboundOrderId",
                principalTable: "OutboundOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OutboundOrders_Suppliers_SupplierId",
                table: "OutboundOrders",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Requisitions_OutboundOrders_OutboundOrderId",
                table: "Requisitions",
                column: "OutboundOrderId",
                principalTable: "OutboundOrders",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OutboundOrderItem_OutboundOrders_OutboundOrderId",
                table: "OutboundOrderItem");

            migrationBuilder.DropForeignKey(
                name: "FK_OutboundOrders_Suppliers_SupplierId",
                table: "OutboundOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_Requisitions_OutboundOrders_OutboundOrderId",
                table: "Requisitions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OutboundOrders",
                table: "OutboundOrders");

            migrationBuilder.RenameTable(
                name: "OutboundOrders",
                newName: "OutboundOrder");

            migrationBuilder.RenameIndex(
                name: "IX_OutboundOrders_SupplierId",
                table: "OutboundOrder",
                newName: "IX_OutboundOrder_SupplierId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OutboundOrder",
                table: "OutboundOrder",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OutboundOrder_Suppliers_SupplierId",
                table: "OutboundOrder",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OutboundOrderItem_OutboundOrder_OutboundOrderId",
                table: "OutboundOrderItem",
                column: "OutboundOrderId",
                principalTable: "OutboundOrder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Requisitions_OutboundOrder_OutboundOrderId",
                table: "Requisitions",
                column: "OutboundOrderId",
                principalTable: "OutboundOrder",
                principalColumn: "Id");
        }
    }
}
