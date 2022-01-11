using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Award.Infrastructure.Data.Migrations
{
    public partial class addwinner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Winners",
                schema: "award",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    IsAnnounced = table.Column<bool>(nullable: false),
                    ShowWinnerImage = table.Column<bool>(nullable: false),
                    AnounceDate = table.Column<DateTime>(nullable: true),
                    IdAward = table.Column<long>(nullable: false),
                    IdCategory = table.Column<long>(nullable: false),
                    IdEmployee = table.Column<long>(nullable: true),
                    IdSector = table.Column<long>(nullable: true),
                    CategoryId = table.Column<long>(nullable: true),
                    AwardId = table.Column<long>(nullable: true),
                    EmployeeId = table.Column<long>(nullable: true),
                    SectorId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Winners", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Winners_Awards_AwardId",
                        column: x => x.AwardId,
                        principalSchema: "award",
                        principalTable: "Awards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Winners_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "award",
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Winners_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "award",
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Winners_Sectors_SectorId",
                        column: x => x.SectorId,
                        principalSchema: "award",
                        principalTable: "Sectors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Winners_AwardId",
                schema: "award",
                table: "Winners",
                column: "AwardId");

            migrationBuilder.CreateIndex(
                name: "IX_Winners_CategoryId",
                schema: "award",
                table: "Winners",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Winners_EmployeeId",
                schema: "award",
                table: "Winners",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Winners_SectorId",
                schema: "award",
                table: "Winners",
                column: "SectorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Winners",
                schema: "award");
        }
    }
}
