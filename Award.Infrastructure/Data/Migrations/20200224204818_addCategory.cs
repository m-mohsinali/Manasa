using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Award.Infrastructure.Data.Migrations
{
    public partial class addCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    IdAward = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    AwardType = table.Column<int>(nullable: false),
                    OpeningDate = table.Column<DateTime>(nullable: false),
                    ClosingDate = table.Column<DateTime>(nullable: false),
                    AwardId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_Awards_AwardId",
                        column: x => x.AwardId,
                        principalTable: "Awards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CategoryCriterias",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCategory = table.Column<long>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    CategoryId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryCriterias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoryCriterias_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CategoryDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCategory = table.Column<long>(nullable: false),
                    SupportingDocumentPath = table.Column<string>(nullable: true),
                    CategoryId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoryDocuments_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CategoryCriteriaDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupportingDocumentPath = table.Column<string>(nullable: true),
                    IdCategoryCriteria = table.Column<long>(nullable: false),
                    CategoryCriteriaId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryCriteriaDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoryCriteriaDocuments_CategoryCriterias_CategoryCriteriaId",
                        column: x => x.CategoryCriteriaId,
                        principalTable: "CategoryCriterias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CategorySubCriterias",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(nullable: false),
                    Marks = table.Column<short>(nullable: false),
                    MaxWords = table.Column<short>(nullable: false),
                    IdCategoryCriteria = table.Column<long>(nullable: false),
                    CategoryCriteriaId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategorySubCriterias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategorySubCriterias_CategoryCriterias_CategoryCriteriaId",
                        column: x => x.CategoryCriteriaId,
                        principalTable: "CategoryCriterias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_AwardId",
                table: "Categories",
                column: "AwardId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryCriteriaDocuments_CategoryCriteriaId",
                table: "CategoryCriteriaDocuments",
                column: "CategoryCriteriaId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryCriterias_CategoryId",
                table: "CategoryCriterias",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryDocuments_CategoryId",
                table: "CategoryDocuments",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CategorySubCriterias_CategoryCriteriaId",
                table: "CategorySubCriterias",
                column: "CategoryCriteriaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryCriteriaDocuments");

            migrationBuilder.DropTable(
                name: "CategoryDocuments");

            migrationBuilder.DropTable(
                name: "CategorySubCriterias");

            migrationBuilder.DropTable(
                name: "CategoryCriterias");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
