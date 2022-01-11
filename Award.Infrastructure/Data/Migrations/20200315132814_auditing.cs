using Microsoft.EntityFrameworkCore.Migrations;

namespace Award.Infrastructure.Data.Migrations
{
    public partial class auditing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryId",
                schema: "award",
                table: "CategoryTeamEntries");

            migrationBuilder.AddColumn<long>(
                name: "EntryId",
                schema: "award",
                table: "CategoryTeamEntries",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EntryId",
                schema: "award",
                table: "CategoryTeamEntries");

            migrationBuilder.AddColumn<long>(
                name: "CategoryId",
                schema: "award",
                table: "CategoryTeamEntries",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
