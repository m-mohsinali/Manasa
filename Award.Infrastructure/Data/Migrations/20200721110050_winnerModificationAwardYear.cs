using Microsoft.EntityFrameworkCore.Migrations;

namespace Award.Infrastructure.Data.Migrations
{
    public partial class winnerModificationAwardYear : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AwardId",
                schema: "award",
                table: "WinnerAnnouncements",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AwardYear",
                schema: "award",
                table: "WinnerAnnouncements",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CategoryId",
                schema: "award",
                table: "WinnerAnnouncements",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WinnerAnnouncements_AwardId",
                schema: "award",
                table: "WinnerAnnouncements",
                column: "AwardId");

            migrationBuilder.CreateIndex(
                name: "IX_WinnerAnnouncements_CategoryId",
                schema: "award",
                table: "WinnerAnnouncements",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_WinnerAnnouncements_Awards_AwardId",
                schema: "award",
                table: "WinnerAnnouncements",
                column: "AwardId",
                principalSchema: "award",
                principalTable: "Awards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WinnerAnnouncements_Categories_CategoryId",
                schema: "award",
                table: "WinnerAnnouncements",
                column: "CategoryId",
                principalSchema: "award",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WinnerAnnouncements_Awards_AwardId",
                schema: "award",
                table: "WinnerAnnouncements");

            migrationBuilder.DropForeignKey(
                name: "FK_WinnerAnnouncements_Categories_CategoryId",
                schema: "award",
                table: "WinnerAnnouncements");

            migrationBuilder.DropIndex(
                name: "IX_WinnerAnnouncements_AwardId",
                schema: "award",
                table: "WinnerAnnouncements");

            migrationBuilder.DropIndex(
                name: "IX_WinnerAnnouncements_CategoryId",
                schema: "award",
                table: "WinnerAnnouncements");

            migrationBuilder.DropColumn(
                name: "AwardId",
                schema: "award",
                table: "WinnerAnnouncements");

            migrationBuilder.DropColumn(
                name: "AwardYear",
                schema: "award",
                table: "WinnerAnnouncements");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                schema: "award",
                table: "WinnerAnnouncements");
        }
    }
}
