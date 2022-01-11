using Microsoft.EntityFrameworkCore.Migrations;

namespace Award.Infrastructure.Data.Migrations
{
    public partial class Forumtablesfk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ForumId",
                schema: "award",
                table: "ForumDetails",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ForumDetails_ForumId",
                schema: "award",
                table: "ForumDetails",
                column: "ForumId");

            migrationBuilder.AddForeignKey(
                name: "FK_ForumDetails_Forum_ForumId",
                schema: "award",
                table: "ForumDetails",
                column: "ForumId",
                principalSchema: "award",
                principalTable: "Forum",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ForumDetails_Forum_ForumId",
                schema: "award",
                table: "ForumDetails");

            migrationBuilder.DropIndex(
                name: "IX_ForumDetails_ForumId",
                schema: "award",
                table: "ForumDetails");

            migrationBuilder.DropColumn(
                name: "ForumId",
                schema: "award",
                table: "ForumDetails");
        }
    }
}
