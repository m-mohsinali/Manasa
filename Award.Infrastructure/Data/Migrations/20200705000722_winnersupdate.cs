using Microsoft.EntityFrameworkCore.Migrations;

namespace Award.Infrastructure.Data.Migrations
{
    public partial class winnersupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AwardName",
                schema: "award",
                table: "WinnerAnnouncements",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                schema: "award",
                table: "WinnerAnnouncements",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WinnerAnnouncements_UserId",
                schema: "award",
                table: "WinnerAnnouncements",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_WinnerAnnouncements_Users_UserId",
                schema: "award",
                table: "WinnerAnnouncements",
                column: "UserId",
                principalSchema: "award",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WinnerAnnouncements_Users_UserId",
                schema: "award",
                table: "WinnerAnnouncements");

            migrationBuilder.DropIndex(
                name: "IX_WinnerAnnouncements_UserId",
                schema: "award",
                table: "WinnerAnnouncements");

            migrationBuilder.DropColumn(
                name: "AwardName",
                schema: "award",
                table: "WinnerAnnouncements");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "award",
                table: "WinnerAnnouncements");
        }
    }
}
