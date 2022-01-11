using Microsoft.EntityFrameworkCore.Migrations;

namespace Award.Infrastructure.Data.Migrations
{
    public partial class InsertedNewTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "TeamId",
                schema: "award",
                table: "CategoryTeams",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "QsmEntryId",
                schema: "award",
                table: "CategoryTeams",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "CurrentStatusId",
                schema: "award",
                table: "CategoryTeams",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TeamId",
                schema: "award",
                table: "CategoryTeams",
                type: "int",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<int>(
                name: "QsmEntryId",
                schema: "award",
                table: "CategoryTeams",
                type: "int",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<int>(
                name: "CurrentStatusId",
                schema: "award",
                table: "CategoryTeams",
                type: "int",
                nullable: false,
                oldClrType: typeof(long));
        }
    }
}
