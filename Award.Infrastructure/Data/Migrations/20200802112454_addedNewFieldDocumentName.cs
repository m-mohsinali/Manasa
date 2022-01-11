using Microsoft.EntityFrameworkCore.Migrations;

namespace Award.Infrastructure.Data.Migrations
{
    public partial class addedNewFieldDocumentName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DocumentName",
                schema: "award",
                table: "CriteriaDocuments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DocumentName",
                schema: "award",
                table: "CategoryDocuments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DocumentName",
                schema: "award",
                table: "AwardDocuments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentName",
                schema: "award",
                table: "CriteriaDocuments");

            migrationBuilder.DropColumn(
                name: "DocumentName",
                schema: "award",
                table: "CategoryDocuments");

            migrationBuilder.DropColumn(
                name: "DocumentName",
                schema: "award",
                table: "AwardDocuments");
        }
    }
}
