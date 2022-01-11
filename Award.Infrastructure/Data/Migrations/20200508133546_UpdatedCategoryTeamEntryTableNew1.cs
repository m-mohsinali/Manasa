using Microsoft.EntityFrameworkCore.Migrations;

namespace Award.Infrastructure.Data.Migrations
{
    public partial class UpdatedCategoryTeamEntryTableNew1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "award",
                table: "CategoryTeams",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                schema: "award",
                table: "CategoryTeams");
        }
    }
}
