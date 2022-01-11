using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Award.Infrastructure.Data.Migrations
{
    public partial class migrationQsmSectorAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Sectors_SectorId",
                schema: "award",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_SectorId",
                schema: "award",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "SectorId",
                schema: "award",
                table: "Categories");

            migrationBuilder.CreateTable(
                name: "QsmSector",
                schema: "award",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    SectorId = table.Column<long>(nullable: false),
                    UserID = table.Column<long>(nullable: false),
                    RoleId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QsmSector", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QsmSector",
                schema: "award");

            migrationBuilder.AddColumn<long>(
                name: "SectorId",
                schema: "award",
                table: "Categories",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_SectorId",
                schema: "award",
                table: "Categories",
                column: "SectorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Sectors_SectorId",
                schema: "award",
                table: "Categories",
                column: "SectorId",
                principalSchema: "award",
                principalTable: "Sectors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
