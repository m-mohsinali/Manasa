using Microsoft.EntityFrameworkCore.Migrations;

namespace Award.Infrastructure.Data.Migrations
{
    public partial class fix_award_documents_on_delete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AwardDocuments_Awards_AwardsId",
                schema: "award",
                table: "AwardDocuments");

            migrationBuilder.AddForeignKey(
                name: "FK_AwardDocuments_Awards_AwardsId",
                schema: "award",
                table: "AwardDocuments",
                column: "AwardsId",
                principalSchema: "award",
                principalTable: "Awards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AwardDocuments_Awards_AwardsId",
                schema: "award",
                table: "AwardDocuments");

            migrationBuilder.AddForeignKey(
                name: "FK_AwardDocuments_Awards_AwardsId",
                schema: "award",
                table: "AwardDocuments",
                column: "AwardsId",
                principalSchema: "award",
                principalTable: "Awards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
