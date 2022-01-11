using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Award.Infrastructure.Data.Migrations
{
    public partial class removedTableQsmSector : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoriesSectors_Categories_CategoryId",
                schema: "award",
                table: "CategoriesSectors");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoriesSectors_Sectors_SectorId",
                schema: "award",
                table: "CategoriesSectors");

            migrationBuilder.DropTable(
                name: "QsmSector",
                schema: "award");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoriesSectors",
                schema: "award",
                table: "CategoriesSectors");

            migrationBuilder.RenameTable(
                name: "CategoriesSectors",
                schema: "award",
                newName: "CategorySector",
                newSchema: "award");

            migrationBuilder.RenameIndex(
                name: "IX_CategoriesSectors_SectorId",
                schema: "award",
                table: "CategorySector",
                newName: "IX_CategorySector_SectorId");

            migrationBuilder.RenameIndex(
                name: "IX_CategoriesSectors_CategoryId",
                schema: "award",
                table: "CategorySector",
                newName: "IX_CategorySector_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategorySector",
                schema: "award",
                table: "CategorySector",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CategorySector_Categories_CategoryId",
                schema: "award",
                table: "CategorySector",
                column: "CategoryId",
                principalSchema: "award",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CategorySector_Sectors_SectorId",
                schema: "award",
                table: "CategorySector",
                column: "SectorId",
                principalSchema: "award",
                principalTable: "Sectors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategorySector_Categories_CategoryId",
                schema: "award",
                table: "CategorySector");

            migrationBuilder.DropForeignKey(
                name: "FK_CategorySector_Sectors_SectorId",
                schema: "award",
                table: "CategorySector");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CategorySector",
                schema: "award",
                table: "CategorySector");

            migrationBuilder.RenameTable(
                name: "CategorySector",
                schema: "award",
                newName: "CategoriesSectors",
                newSchema: "award");

            migrationBuilder.RenameIndex(
                name: "IX_CategorySector_SectorId",
                schema: "award",
                table: "CategoriesSectors",
                newName: "IX_CategoriesSectors_SectorId");

            migrationBuilder.RenameIndex(
                name: "IX_CategorySector_CategoryId",
                schema: "award",
                table: "CategoriesSectors",
                newName: "IX_CategoriesSectors_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoriesSectors",
                schema: "award",
                table: "CategoriesSectors",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "QsmSector",
                schema: "award",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    SectorId = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QsmSector", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_CategoriesSectors_Categories_CategoryId",
                schema: "award",
                table: "CategoriesSectors",
                column: "CategoryId",
                principalSchema: "award",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoriesSectors_Sectors_SectorId",
                schema: "award",
                table: "CategoriesSectors",
                column: "SectorId",
                principalSchema: "award",
                principalTable: "Sectors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
