using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Award.Infrastructure.Data.Migrations
{
    public partial class Forumtables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Forum",
                schema: "award",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    ForumText = table.Column<string>(nullable: true),
                    CategoryId = table.Column<long>(nullable: true),
                    UserId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Forum", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Forum_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "award",
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Forum_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "award",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ForumDetails",
                schema: "award",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    ForumDetailText = table.Column<string>(nullable: true),
                    UserId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForumDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ForumDetails_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "award",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Forum_CategoryId",
                schema: "award",
                table: "Forum",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Forum_UserId",
                schema: "award",
                table: "Forum",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ForumDetails_UserId",
                schema: "award",
                table: "ForumDetails",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Forum",
                schema: "award");

            migrationBuilder.DropTable(
                name: "ForumDetails",
                schema: "award");
        }
    }
}
