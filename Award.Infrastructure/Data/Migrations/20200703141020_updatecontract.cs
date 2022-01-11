using Microsoft.EntityFrameworkCore.Migrations;

namespace Award.Infrastructure.Data.Migrations
{
    public partial class updatecontract : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmployeeNameAr",
                schema: "award",
                table: "Contacts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeNameEn",
                schema: "award",
                table: "Contacts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeNameAr",
                schema: "award",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "EmployeeNameEn",
                schema: "award",
                table: "Contacts");
        }
    }
}
