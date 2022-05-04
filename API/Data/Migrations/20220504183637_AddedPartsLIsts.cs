using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    public partial class AddedPartsLIsts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PartsLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartsLists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PartsListEntry",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PartId = table.Column<int>(type: "INTEGER", nullable: false),
                    PartsListId = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<float>(type: "REAL", nullable: false),
                    ComponentLocation = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartsListEntry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PartsListEntry_Parts_PartId",
                        column: x => x.PartId,
                        principalTable: "Parts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_PartsListEntry_PartsLists_PartsListId",
                        column: x => x.PartsListId,
                        principalTable: "PartsLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PartsListEntry_PartId",
                table: "PartsListEntry",
                column: "PartId");

            migrationBuilder.CreateIndex(
                name: "IX_PartsListEntry_PartsListId",
                table: "PartsListEntry",
                column: "PartsListId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PartsListEntry");

            migrationBuilder.DropTable(
                name: "PartsLists");
        }
    }
}
