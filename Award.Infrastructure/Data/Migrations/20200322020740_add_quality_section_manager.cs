using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Award.Infrastructure.Data.Migrations
{
    public partial class add_quality_section_manager : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QualitySectorManagers",
                schema: "award",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    SectorId = table.Column<long>(nullable: false),
                    UserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QualitySectorManagers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QualitySectorManagers_Sectors_SectorId",
                        column: x => x.SectorId,
                        principalSchema: "award",
                        principalTable: "Sectors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QualitySectorManagers_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "award",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QualitySectorManagers_UserId",
                schema: "award",
                table: "QualitySectorManagers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_QualitySectorManagers_SectorId_UserId",
                schema: "award",
                table: "QualitySectorManagers",
                columns: new[] { "SectorId", "UserId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QualitySectorManagers",
                schema: "award");
        }
    }
}
