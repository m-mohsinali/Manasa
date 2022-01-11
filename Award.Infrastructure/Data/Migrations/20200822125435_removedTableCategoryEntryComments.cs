using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Award.Infrastructure.Data.Migrations
{
    public partial class removedTableCategoryEntryComments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryEmployee",
                schema: "award");

            migrationBuilder.DropTable(
                name: "CategoryEntryComments",
                schema: "award");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoryEmployee",
                schema: "award",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssignedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdCategory = table.Column<long>(type: "bigint", nullable: false),
                    IdCategoryNavigationId = table.Column<long>(type: "bigint", nullable: true),
                    IdEmployee = table.Column<long>(type: "bigint", nullable: false),
                    IdEmployeeNavigationId = table.Column<long>(type: "bigint", nullable: true),
                    NotifiedTries = table.Column<short>(type: "smallint", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryEmployee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoryEmployee_Categories_IdCategoryNavigationId",
                        column: x => x.IdCategoryNavigationId,
                        principalSchema: "award",
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CategoryEmployee_Employee_IdEmployeeNavigationId",
                        column: x => x.IdEmployeeNavigationId,
                        principalSchema: "award",
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CategoryEntryComments",
                schema: "award",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryEntryComments", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryEmployee_IdCategoryNavigationId",
                schema: "award",
                table: "CategoryEmployee",
                column: "IdCategoryNavigationId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryEmployee_IdEmployeeNavigationId",
                schema: "award",
                table: "CategoryEmployee",
                column: "IdEmployeeNavigationId");
        }
    }
}
