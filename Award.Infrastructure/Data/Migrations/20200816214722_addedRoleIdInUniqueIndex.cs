using Microsoft.EntityFrameworkCore.Migrations;

namespace Award.Infrastructure.Data.Migrations
{
    public partial class addedRoleIdInUniqueIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EmployeeTeams_UserId_TeamId",
                schema: "award",
                table: "EmployeeTeams");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeTeams_UserId_TeamId_RoleId",
                schema: "award",
                table: "EmployeeTeams",
                columns: new[] { "UserId", "TeamId", "RoleId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EmployeeTeams_UserId_TeamId_RoleId",
                schema: "award",
                table: "EmployeeTeams");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeTeams_UserId_TeamId",
                schema: "award",
                table: "EmployeeTeams",
                columns: new[] { "UserId", "TeamId" },
                unique: true);
        }
    }
}
