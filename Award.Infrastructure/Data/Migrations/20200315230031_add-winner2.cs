using Microsoft.EntityFrameworkCore.Migrations;

namespace Award.Infrastructure.Data.Migrations
{
    public partial class addwinner2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Winners_Awards_AwardId",
                schema: "award",
                table: "Winners");

            migrationBuilder.DropForeignKey(
                name: "FK_Winners_Categories_CategoryId",
                schema: "award",
                table: "Winners");

            migrationBuilder.DropColumn(
                name: "IdAward",
                schema: "award",
                table: "Winners");

            migrationBuilder.DropColumn(
                name: "IdCategory",
                schema: "award",
                table: "Winners");

            migrationBuilder.DropColumn(
                name: "IdEmployee",
                schema: "award",
                table: "Winners");

            migrationBuilder.DropColumn(
                name: "IdSector",
                schema: "award",
                table: "Winners");

            migrationBuilder.AlterColumn<long>(
                name: "CategoryId",
                schema: "award",
                table: "Winners",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "AwardId",
                schema: "award",
                table: "Winners",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Winners_Awards_AwardId",
                schema: "award",
                table: "Winners",
                column: "AwardId",
                principalSchema: "award",
                principalTable: "Awards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Winners_Categories_CategoryId",
                schema: "award",
                table: "Winners",
                column: "CategoryId",
                principalSchema: "award",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Winners_Awards_AwardId",
                schema: "award",
                table: "Winners");

            migrationBuilder.DropForeignKey(
                name: "FK_Winners_Categories_CategoryId",
                schema: "award",
                table: "Winners");

            migrationBuilder.AlterColumn<long>(
                name: "CategoryId",
                schema: "award",
                table: "Winners",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<long>(
                name: "AwardId",
                schema: "award",
                table: "Winners",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<long>(
                name: "IdAward",
                schema: "award",
                table: "Winners",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "IdCategory",
                schema: "award",
                table: "Winners",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "IdEmployee",
                schema: "award",
                table: "Winners",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "IdSector",
                schema: "award",
                table: "Winners",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Winners_Awards_AwardId",
                schema: "award",
                table: "Winners",
                column: "AwardId",
                principalSchema: "award",
                principalTable: "Awards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Winners_Categories_CategoryId",
                schema: "award",
                table: "Winners",
                column: "CategoryId",
                principalSchema: "award",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
