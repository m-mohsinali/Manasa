using Microsoft.EntityFrameworkCore.Migrations;

namespace Award.Infrastructure.Data.Migrations
{
    public partial class ChangedIdOfTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryDocuments_Categories_CategoryId",
                schema: "award",
                table: "CategoryDocuments");

            migrationBuilder.DropColumn(
                name: "IdCategory",
                schema: "award",
                table: "CategoryDocuments");

            migrationBuilder.AlterColumn<long>(
                name: "CategoryId",
                schema: "award",
                table: "CategoryDocuments",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryDocuments_Categories_CategoryId",
                schema: "award",
                table: "CategoryDocuments",
                column: "CategoryId",
                principalSchema: "award",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryDocuments_Categories_CategoryId",
                schema: "award",
                table: "CategoryDocuments");

            migrationBuilder.AlterColumn<long>(
                name: "CategoryId",
                schema: "award",
                table: "CategoryDocuments",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<long>(
                name: "IdCategory",
                schema: "award",
                table: "CategoryDocuments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryDocuments_Categories_CategoryId",
                schema: "award",
                table: "CategoryDocuments",
                column: "CategoryId",
                principalSchema: "award",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
