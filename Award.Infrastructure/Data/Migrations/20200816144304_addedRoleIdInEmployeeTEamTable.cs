using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Award.Infrastructure.Data.Migrations
{
    public partial class addedRoleIdInEmployeeTEamTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "vwManasaEmployees",
                schema: "award");

            migrationBuilder.AddColumn<long>(
                name: "RoleId",
                schema: "award",
                table: "EmployeeTeams",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoleId",
                schema: "award",
                table: "EmployeeTeams");

            migrationBuilder.CreateTable(
                name: "vwManasaEmployees",
                schema: "award",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchNameAR = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BranchNameEN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClassAR = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClassEN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeptNameAR = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeptNameEN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeePhotoURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GRP = table.Column<int>(type: "int", nullable: true),
                    JobTitleAR = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobTitleEN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdaeDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NameAR = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameEN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RankAR = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RankEN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SectionNameAR = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SectionNameEN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SectorNameAR = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SectorNameEN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SexAR = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SexEN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitNameAR = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitNameEN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserDomain = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vwManasaEmployees", x => x.ID);
                });
        }
    }
}
