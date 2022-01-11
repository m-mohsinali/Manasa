using Microsoft.EntityFrameworkCore.Migrations;

namespace Award.Infrastructure.Data.Migrations
{
    public partial class UpdatedCategoryTeamTableNew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryTeams_Categories_CategoryId",
                schema: "award",
                table: "CategoryTeams");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryTeams_Users_UserId",
                schema: "award",
                table: "CategoryTeams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoryTeams",
                schema: "award",
                table: "CategoryTeams");

            migrationBuilder.DropIndex(
                name: "IX_CategoryTeams_CategoryId",
                schema: "award",
                table: "CategoryTeams");

            migrationBuilder.DropIndex(
                name: "IX_CategoryTeams_UserId",
                schema: "award",
                table: "CategoryTeams");

            migrationBuilder.DropColumn(
                name: "IdTeams",
                schema: "award",
                table: "CategoryTeams");

            migrationBuilder.DropColumn(
                name: "IdCategory",
                schema: "award",
                table: "CategoryTeams");

            migrationBuilder.DropColumn(
                name: "IdUser",
                schema: "award",
                table: "CategoryTeams");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                schema: "award",
                table: "CategoryTeams",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CategoryId",
                schema: "award",
                table: "CategoryTeams",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TeamId",
                schema: "award",
                table: "CategoryTeams",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoryTeams",
                schema: "award",
                table: "CategoryTeams",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoryTeams",
                schema: "award",
                table: "CategoryTeams");

            migrationBuilder.DropColumn(
                name: "TeamId",
                schema: "award",
                table: "CategoryTeams");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                schema: "award",
                table: "CategoryTeams",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<long>(
                name: "CategoryId",
                schema: "award",
                table: "CategoryTeams",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<int>(
                name: "IdTeams",
                schema: "award",
                table: "CategoryTeams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "IdCategory",
                schema: "award",
                table: "CategoryTeams",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "IdUser",
                schema: "award",
                table: "CategoryTeams",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoryTeams",
                schema: "award",
                table: "CategoryTeams",
                columns: new[] { "IdTeams", "IdCategory", "IdUser" });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryTeams_CategoryId",
                schema: "award",
                table: "CategoryTeams",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryTeams_UserId",
                schema: "award",
                table: "CategoryTeams",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryTeams_Categories_CategoryId",
                schema: "award",
                table: "CategoryTeams",
                column: "CategoryId",
                principalSchema: "award",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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
    }
}
