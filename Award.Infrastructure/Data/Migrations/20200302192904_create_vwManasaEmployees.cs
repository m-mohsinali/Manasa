using Microsoft.EntityFrameworkCore.Migrations;

namespace Award.Infrastructure.Data.Migrations
{
    public partial class create_vwManasaEmployees : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ManasaEmployees",
                table: "ManasaEmployees");

            migrationBuilder.RenameTable(
                name: "ManasaEmployees",
                newName: "vwManasaEmployees");

            migrationBuilder.AddPrimaryKey(
                name: "PK_vwManasaEmployees",
                table: "vwManasaEmployees",
                column: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_vwManasaEmployees",
                table: "vwManasaEmployees");

            migrationBuilder.RenameTable(
                name: "vwManasaEmployees",
                newName: "ManasaEmployees");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ManasaEmployees",
                table: "ManasaEmployees",
                column: "ID");
        }
    }
}
