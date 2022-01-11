using Microsoft.EntityFrameworkCore.Migrations;

namespace Award.Infrastructure.Data.Migrations
{
    public partial class cascade_on_delete_for_critirea : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryCriterias_Categories_CategoryId",
                table: "CategoryCriterias");

            migrationBuilder.DropForeignKey(
                name: "FK_CategorySubCriterias_CategoryCriterias_CategoryCriteriaId",
                table: "CategorySubCriterias");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryCriterias_Categories_CategoryId",
                table: "CategoryCriterias",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CategorySubCriterias_CategoryCriterias_CategoryCriteriaId",
                table: "CategorySubCriterias",
                column: "CategoryCriteriaId",
                principalTable: "CategoryCriterias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryCriterias_Categories_CategoryId",
                table: "CategoryCriterias");

            migrationBuilder.DropForeignKey(
                name: "FK_CategorySubCriterias_CategoryCriterias_CategoryCriteriaId",
                table: "CategorySubCriterias");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryCriterias_Categories_CategoryId",
                table: "CategoryCriterias",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CategorySubCriterias_CategoryCriterias_CategoryCriteriaId",
                table: "CategorySubCriterias",
                column: "CategoryCriteriaId",
                principalTable: "CategoryCriterias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
