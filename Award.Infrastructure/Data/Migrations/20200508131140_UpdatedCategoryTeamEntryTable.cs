using Microsoft.EntityFrameworkCore.Migrations;

namespace Award.Infrastructure.Data.Migrations
{
    public partial class UpdatedCategoryTeamEntryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Branches_BranchId",
                schema: "award",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Departments_DepartmentId",
                schema: "award",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Sections_SectionId",
                schema: "award",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Units_UnitId",
                schema: "award",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                schema: "award",
                table: "CategoryTeams");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "award",
                table: "CategoryTeams");

            migrationBuilder.AlterColumn<long>(
                name: "UnitId",
                schema: "award",
                table: "Employee",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "SectionId",
                schema: "award",
                table: "Employee",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "DepartmentId",
                schema: "award",
                table: "Employee",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "BranchId",
                schema: "award",
                table: "Employee",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<int>(
                name: "QsmEntryId",
                schema: "award",
                table: "CategoryTeams",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Branches_BranchId",
                schema: "award",
                table: "Employee",
                column: "BranchId",
                principalSchema: "award",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Departments_DepartmentId",
                schema: "award",
                table: "Employee",
                column: "DepartmentId",
                principalSchema: "award",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Sections_SectionId",
                schema: "award",
                table: "Employee",
                column: "SectionId",
                principalSchema: "award",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Units_UnitId",
                schema: "award",
                table: "Employee",
                column: "UnitId",
                principalSchema: "award",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Branches_BranchId",
                schema: "award",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Departments_DepartmentId",
                schema: "award",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Sections_SectionId",
                schema: "award",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Units_UnitId",
                schema: "award",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "QsmEntryId",
                schema: "award",
                table: "CategoryTeams");

            migrationBuilder.AlterColumn<long>(
                name: "UnitId",
                schema: "award",
                table: "Employee",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "SectionId",
                schema: "award",
                table: "Employee",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "DepartmentId",
                schema: "award",
                table: "Employee",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "BranchId",
                schema: "award",
                table: "Employee",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CategoryId",
                schema: "award",
                table: "CategoryTeams",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                schema: "award",
                table: "CategoryTeams",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Branches_BranchId",
                schema: "award",
                table: "Employee",
                column: "BranchId",
                principalSchema: "award",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Departments_DepartmentId",
                schema: "award",
                table: "Employee",
                column: "DepartmentId",
                principalSchema: "award",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Sections_SectionId",
                schema: "award",
                table: "Employee",
                column: "SectionId",
                principalSchema: "award",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Units_UnitId",
                schema: "award",
                table: "Employee",
                column: "UnitId",
                principalSchema: "award",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
