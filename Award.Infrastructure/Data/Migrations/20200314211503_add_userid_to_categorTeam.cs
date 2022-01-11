using Microsoft.EntityFrameworkCore.Migrations;

namespace Award.Infrastructure.Data.Migrations
{
    public partial class add_userid_to_categorTeam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoryTeams",
                schema: "award",
                table: "CategoryTeams");

            migrationBuilder.AddColumn<long>(
                name: "IdUser",
                schema: "award",
                table: "CategoryTeams",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                schema: "award",
                table: "CategoryTeams",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoryTeams",
                schema: "award",
                table: "CategoryTeams",
                columns: new[] { "IdTeams", "IdCategory", "IdUser" });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryTeams_UserId",
                schema: "award",
                table: "CategoryTeams",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryTeams_Users_UserId",
                schema: "award",
                table: "CategoryTeams",
                column: "UserId",
                principalSchema: "award",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryTeams_Users_UserId",
                schema: "award",
                table: "CategoryTeams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoryTeams",
                schema: "award",
                table: "CategoryTeams");

            migrationBuilder.DropIndex(
                name: "IX_CategoryTeams_UserId",
                schema: "award",
                table: "CategoryTeams");

            migrationBuilder.DropColumn(
                name: "IdUser",
                schema: "award",
                table: "CategoryTeams");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "award",
                table: "CategoryTeams");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoryTeams",
                schema: "award",
                table: "CategoryTeams",
                columns: new[] { "IdTeams", "IdCategory" });
        }
    }
}
