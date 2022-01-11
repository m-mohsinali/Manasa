using Microsoft.EntityFrameworkCore.Migrations;

namespace Award.Infrastructure.Data.Migrations
{
    public partial class AddedRelationFromUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeTeams_Users_UserId",
                schema: "award",
                table: "EmployeeTeams",
                column: "UserId",
                principalSchema: "award",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeTeams_Users_UserId",
                schema: "award",
                table: "EmployeeTeams");
        }
    }
}
