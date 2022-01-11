using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Award.Infrastructure.Data.Migrations
{
    public partial class qsmEntryanswers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QSMEntries",
                schema: "award",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    CategoryId = table.Column<long>(nullable: false),
                    SubmittedUserId = table.Column<long>(nullable: false),
                    AssignedUserId = table.Column<long>(nullable: false),
                    CurrentStatusId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QSMEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserAnswers",
                schema: "award",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    QsmEntryId = table.Column<long>(nullable: false),
                    CategoryId = table.Column<long>(nullable: false),
                    CriteriaId = table.Column<long>(nullable: false),
                    SubCriteriaId = table.Column<long>(nullable: false),
                    Answer = table.Column<string>(nullable: true),
                    CurrentStatusId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAnswers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserAnswersComments",
                schema: "award",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    UserAnswerId = table.Column<long>(nullable: false),
                    Comments = table.Column<string>(nullable: true),
                    CurrentStatusId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAnswersComments", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QSMEntries",
                schema: "award");

            migrationBuilder.DropTable(
                name: "UserAnswers",
                schema: "award");

            migrationBuilder.DropTable(
                name: "UserAnswersComments",
                schema: "award");
        }
    }
}
