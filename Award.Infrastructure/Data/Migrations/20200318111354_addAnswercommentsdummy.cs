using Microsoft.EntityFrameworkCore.Migrations;

namespace Award.Infrastructure.Data.Migrations
{
    public partial class addAnswercommentsdummy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Answer",
                schema: "award",
                table: "CategorySubCriterias",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "Comments",
                schema: "award",
                table: "CategorySubCriterias",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Answer",
                schema: "award",
                table: "CategorySubCriterias");

            migrationBuilder.DropColumn(
                name: "Comments",
                schema: "award",
                table: "CategorySubCriterias");
        }
    }
}
