using Microsoft.EntityFrameworkCore.Migrations;

namespace Award.Infrastructure.Data.Migrations
{
    public partial class categoryidAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CategoryId",
                schema: "award",
                table: "CategoryEntries",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryId",
                schema: "award",
                table: "CategoryEntries");
        }
    }
}
