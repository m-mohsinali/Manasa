using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Award.Infrastructure.Data.Migrations
{
    public partial class insertedNewTableCatTeams : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryTeams",
                schema: "award");

            migrationBuilder.CreateTable(
                name: "CategoryTeamEntries",
                schema: "award",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    TeamId = table.Column<long>(nullable: false),
                    QsmEntryId = table.Column<long>(nullable: false),
                    CurrentStatusId = table.Column<long>(nullable: false),
                    TeamsId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryTeamEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoryTeamEntries_Teams_TeamsId",
                        column: x => x.TeamsId,
                        principalSchema: "award",
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryTeamEntries_TeamsId",
                schema: "award",
                table: "CategoryTeamEntries",
                column: "TeamsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryTeamEntries",
                schema: "award");

            migrationBuilder.CreateTable(
                name: "CategoryTeams",
                schema: "award",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CurrentStatusId = table.Column<long>(type: "bigint", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    QsmEntryId = table.Column<long>(type: "bigint", nullable: false),
                    TeamId = table.Column<long>(type: "bigint", nullable: false),
                    TeamsId = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryTeams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoryTeams_Teams_TeamsId",
                        column: x => x.TeamsId,
                        principalSchema: "award",
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryTeams_TeamsId",
                schema: "award",
                table: "CategoryTeams",
                column: "TeamsId");
        }
    }
}
