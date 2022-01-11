using Microsoft.EntityFrameworkCore.Migrations;

namespace Award.Infrastructure.Data.Migrations
{
    public partial class migrationSaad : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "SectorId",
                schema: "award",
                table: "Categories",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_SectorId",
                schema: "award",
                table: "Categories",
                column: "SectorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Sectors_SectorId",
                schema: "award",
                table: "Categories",
                column: "SectorId",
                principalSchema: "award",
                principalTable: "Sectors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Sectors_SectorId",
                schema: "award",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_SectorId",
                schema: "award",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "SectorId",
                schema: "award",
                table: "Categories");
        }
    }
}
