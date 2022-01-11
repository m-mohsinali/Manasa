using Microsoft.EntityFrameworkCore.Migrations;

namespace Award.Infrastructure.Data.Migrations
{
    public partial class CategoryId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdCategoryCriteria",
                schema: "award",
                table: "CategorySubCriterias");

            migrationBuilder.AlterColumn<long>(
                name: "CategoryCriteriaId",
                schema: "award",
                table: "CategorySubCriterias",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CategoryId",
                schema: "award",
                table: "CategoryCriterias",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "CategoryCriteriaId",
                schema: "award",
                table: "CategorySubCriterias",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<long>(
                name: "IdCategoryCriteria",
                schema: "award",
                table: "CategorySubCriterias",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<long>(
                name: "CategoryId",
                schema: "award",
                table: "CategoryCriterias",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long));
        }
    }
}
