using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class NullableOutboundId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requisitions_OutboundOrder_OutboundOrderId",
                table: "Requisitions");

            migrationBuilder.AlterColumn<int>(
                name: "OutboundOrderId",
                table: "Requisitions",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Requisitions_OutboundOrder_OutboundOrderId",
                table: "Requisitions",
                column: "OutboundOrderId",
                principalTable: "OutboundOrder",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requisitions_OutboundOrder_OutboundOrderId",
                table: "Requisitions");

            migrationBuilder.AlterColumn<int>(
                name: "OutboundOrderId",
                table: "Requisitions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Requisitions_OutboundOrder_OutboundOrderId",
                table: "Requisitions",
                column: "OutboundOrderId",
                principalTable: "OutboundOrder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
