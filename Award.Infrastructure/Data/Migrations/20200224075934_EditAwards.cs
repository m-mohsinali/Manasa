using Microsoft.EntityFrameworkCore.Migrations;

namespace Award.Infrastructure.Data.Migrations
{
    public partial class EditAwards : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentPath",
                table: "Awards");

            migrationBuilder.AddColumn<int>(
                name: "AwardStatus",
                table: "Awards",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AwardDocuments",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdAwards = table.Column<long>(nullable: false),
                    SupportingDocumentPath = table.Column<string>(nullable: true),
                    AwardsId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AwardDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AwardDocuments_Awards_AwardsId",
                        column: x => x.AwardsId,
                        principalTable: "Awards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AwardDocuments_AwardsId",
                table: "AwardDocuments",
                column: "AwardsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AwardDocuments");

            migrationBuilder.DropColumn(
                name: "AwardStatus",
                table: "Awards");

            migrationBuilder.AddColumn<string>(
                name: "DocumentPath",
                table: "Awards",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
