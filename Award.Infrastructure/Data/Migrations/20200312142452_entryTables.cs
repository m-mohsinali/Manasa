using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Award.Infrastructure.Data.Migrations
{
    public partial class entryTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "award");

            migrationBuilder.RenameTable(
                name: "vwManasaEmployees",
                newName: "vwManasaEmployees",
                newSchema: "award");

            migrationBuilder.RenameTable(
                name: "VideosHome",
                newName: "VideosHome",
                newSchema: "award");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "Users",
                newSchema: "award");

            migrationBuilder.RenameTable(
                name: "UserRoles",
                newName: "UserRoles",
                newSchema: "award");

            migrationBuilder.RenameTable(
                name: "Units",
                newName: "Units",
                newSchema: "award");

            migrationBuilder.RenameTable(
                name: "Teams",
                newName: "Teams",
                newSchema: "award");

            migrationBuilder.RenameTable(
                name: "Sectors",
                newName: "Sectors",
                newSchema: "award");

            migrationBuilder.RenameTable(
                name: "Sections",
                newName: "Sections",
                newSchema: "award");

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "Roles",
                newSchema: "award");

            migrationBuilder.RenameTable(
                name: "EmployeeTeams",
                newName: "EmployeeTeams",
                newSchema: "award");

            migrationBuilder.RenameTable(
                name: "Employee",
                newName: "Employee",
                newSchema: "award");

            migrationBuilder.RenameTable(
                name: "Departments",
                newName: "Departments",
                newSchema: "award");

            migrationBuilder.RenameTable(
                name: "CategoryTeams",
                newName: "CategoryTeams",
                newSchema: "award");

            migrationBuilder.RenameTable(
                name: "CategorySubCriterias",
                newName: "CategorySubCriterias",
                newSchema: "award");

            migrationBuilder.RenameTable(
                name: "CategoryEmployee",
                newName: "CategoryEmployee",
                newSchema: "award");

            migrationBuilder.RenameTable(
                name: "CategoryDocuments",
                newName: "CategoryDocuments",
                newSchema: "award");

            migrationBuilder.RenameTable(
                name: "CategoryCriterias",
                newName: "CategoryCriterias",
                newSchema: "award");

            migrationBuilder.RenameTable(
                name: "CategoryCriteriaDocuments",
                newName: "CategoryCriteriaDocuments",
                newSchema: "award");

            migrationBuilder.RenameTable(
                name: "CategoriesSectors",
                newName: "CategoriesSectors",
                newSchema: "award");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "Categories",
                newSchema: "award");

            migrationBuilder.RenameTable(
                name: "Branches",
                newName: "Branches",
                newSchema: "award");

            migrationBuilder.RenameTable(
                name: "Awards",
                newName: "Awards",
                newSchema: "award");

            migrationBuilder.RenameTable(
                name: "AwardDocuments",
                newName: "AwardDocuments",
                newSchema: "award");

            migrationBuilder.RenameTable(
                name: "AspNetUserTokens",
                newName: "AspNetUserTokens",
                newSchema: "award");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                newName: "AspNetUsers",
                newSchema: "award");

            migrationBuilder.RenameTable(
                name: "AspNetUserRoles",
                newName: "AspNetUserRoles",
                newSchema: "award");

            migrationBuilder.RenameTable(
                name: "AspNetUserLogins",
                newName: "AspNetUserLogins",
                newSchema: "award");

            migrationBuilder.RenameTable(
                name: "AspNetUserClaims",
                newName: "AspNetUserClaims",
                newSchema: "award");

            migrationBuilder.RenameTable(
                name: "AspNetRoles",
                newName: "AspNetRoles",
                newSchema: "award");

            migrationBuilder.RenameTable(
                name: "AspNetRoleClaims",
                newName: "AspNetRoleClaims",
                newSchema: "award");

            migrationBuilder.RenameTable(
                name: "Announcements",
                newName: "Announcements",
                newSchema: "award");

            migrationBuilder.RenameTable(
                name: "AnnouncementHome",
                newName: "AnnouncementHome",
                newSchema: "award");

            migrationBuilder.CreateTable(
                name: "CategoryEntries",
                schema: "award",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    RoleId = table.Column<long>(nullable: false),
                    Comment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CategoryEntryComments",
                schema: "award",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    RoleId = table.Column<long>(nullable: false),
                    Comment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryEntryComments", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryEntries",
                schema: "award");

            migrationBuilder.DropTable(
                name: "CategoryEntryComments",
                schema: "award");

            migrationBuilder.RenameTable(
                name: "vwManasaEmployees",
                schema: "award",
                newName: "vwManasaEmployees");

            migrationBuilder.RenameTable(
                name: "VideosHome",
                schema: "award",
                newName: "VideosHome");

            migrationBuilder.RenameTable(
                name: "Users",
                schema: "award",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "UserRoles",
                schema: "award",
                newName: "UserRoles");

            migrationBuilder.RenameTable(
                name: "Units",
                schema: "award",
                newName: "Units");

            migrationBuilder.RenameTable(
                name: "Teams",
                schema: "award",
                newName: "Teams");

            migrationBuilder.RenameTable(
                name: "Sectors",
                schema: "award",
                newName: "Sectors");

            migrationBuilder.RenameTable(
                name: "Sections",
                schema: "award",
                newName: "Sections");

            migrationBuilder.RenameTable(
                name: "Roles",
                schema: "award",
                newName: "Roles");

            migrationBuilder.RenameTable(
                name: "EmployeeTeams",
                schema: "award",
                newName: "EmployeeTeams");

            migrationBuilder.RenameTable(
                name: "Employee",
                schema: "award",
                newName: "Employee");

            migrationBuilder.RenameTable(
                name: "Departments",
                schema: "award",
                newName: "Departments");

            migrationBuilder.RenameTable(
                name: "CategoryTeams",
                schema: "award",
                newName: "CategoryTeams");

            migrationBuilder.RenameTable(
                name: "CategorySubCriterias",
                schema: "award",
                newName: "CategorySubCriterias");

            migrationBuilder.RenameTable(
                name: "CategoryEmployee",
                schema: "award",
                newName: "CategoryEmployee");

            migrationBuilder.RenameTable(
                name: "CategoryDocuments",
                schema: "award",
                newName: "CategoryDocuments");

            migrationBuilder.RenameTable(
                name: "CategoryCriterias",
                schema: "award",
                newName: "CategoryCriterias");

            migrationBuilder.RenameTable(
                name: "CategoryCriteriaDocuments",
                schema: "award",
                newName: "CategoryCriteriaDocuments");

            migrationBuilder.RenameTable(
                name: "CategoriesSectors",
                schema: "award",
                newName: "CategoriesSectors");

            migrationBuilder.RenameTable(
                name: "Categories",
                schema: "award",
                newName: "Categories");

            migrationBuilder.RenameTable(
                name: "Branches",
                schema: "award",
                newName: "Branches");

            migrationBuilder.RenameTable(
                name: "Awards",
                schema: "award",
                newName: "Awards");

            migrationBuilder.RenameTable(
                name: "AwardDocuments",
                schema: "award",
                newName: "AwardDocuments");

            migrationBuilder.RenameTable(
                name: "AspNetUserTokens",
                schema: "award",
                newName: "AspNetUserTokens");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                schema: "award",
                newName: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "AspNetUserRoles",
                schema: "award",
                newName: "AspNetUserRoles");

            migrationBuilder.RenameTable(
                name: "AspNetUserLogins",
                schema: "award",
                newName: "AspNetUserLogins");

            migrationBuilder.RenameTable(
                name: "AspNetUserClaims",
                schema: "award",
                newName: "AspNetUserClaims");

            migrationBuilder.RenameTable(
                name: "AspNetRoles",
                schema: "award",
                newName: "AspNetRoles");

            migrationBuilder.RenameTable(
                name: "AspNetRoleClaims",
                schema: "award",
                newName: "AspNetRoleClaims");

            migrationBuilder.RenameTable(
                name: "Announcements",
                schema: "award",
                newName: "Announcements");

            migrationBuilder.RenameTable(
                name: "AnnouncementHome",
                schema: "award",
                newName: "AnnouncementHome");
        }
    }
}
