using Microsoft.EntityFrameworkCore.Migrations;

namespace Award.Infrastructure.Data.Migrations
{
    public partial class ChnagedIdsOfEmployeeTable : Migration
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
                name: "FK_Employee_Sectors_SectorId",
                schema: "award",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Units_UnitId",
                schema: "award",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Employee_BranchId",
                schema: "award",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Employee_DepartmentId",
                schema: "award",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Employee_SectionId",
                schema: "award",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Employee_SectorId",
                schema: "award",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Employee_UnitId",
                schema: "award",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "IdBranch",
                schema: "award",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "IdDepartment",
                schema: "award",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "IdSection",
                schema: "award",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "IdSector",
                schema: "award",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "IdUnit",
                schema: "award",
                table: "Employee");

            migrationBuilder.AlterColumn<int>(
                name: "UnitId",
                schema: "award",
                table: "Employee",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SectorId",
                schema: "award",
                table: "Employee",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SectionId",
                schema: "award",
                table: "Employee",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                schema: "award",
                table: "Employee",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BranchId",
                schema: "award",
                table: "Employee",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "BranchId1",
                schema: "award",
                table: "Employee",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DepartmentId1",
                schema: "award",
                table: "Employee",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SectionId1",
                schema: "award",
                table: "Employee",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SectorId1",
                schema: "award",
                table: "Employee",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UnitId1",
                schema: "award",
                table: "Employee",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employee_BranchId1",
                schema: "award",
                table: "Employee",
                column: "BranchId1");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_DepartmentId1",
                schema: "award",
                table: "Employee",
                column: "DepartmentId1");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_SectionId1",
                schema: "award",
                table: "Employee",
                column: "SectionId1");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_SectorId1",
                schema: "award",
                table: "Employee",
                column: "SectorId1");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_UnitId1",
                schema: "award",
                table: "Employee",
                column: "UnitId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Branches_BranchId1",
                schema: "award",
                table: "Employee",
                column: "BranchId1",
                principalSchema: "award",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Departments_DepartmentId1",
                schema: "award",
                table: "Employee",
                column: "DepartmentId1",
                principalSchema: "award",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Sections_SectionId1",
                schema: "award",
                table: "Employee",
                column: "SectionId1",
                principalSchema: "award",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Sectors_SectorId1",
                schema: "award",
                table: "Employee",
                column: "SectorId1",
                principalSchema: "award",
                principalTable: "Sectors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Units_UnitId1",
                schema: "award",
                table: "Employee",
                column: "UnitId1",
                principalSchema: "award",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Branches_BranchId1",
                schema: "award",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Departments_DepartmentId1",
                schema: "award",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Sections_SectionId1",
                schema: "award",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Sectors_SectorId1",
                schema: "award",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Units_UnitId1",
                schema: "award",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Employee_BranchId1",
                schema: "award",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Employee_DepartmentId1",
                schema: "award",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Employee_SectionId1",
                schema: "award",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Employee_SectorId1",
                schema: "award",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Employee_UnitId1",
                schema: "award",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "BranchId1",
                schema: "award",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "DepartmentId1",
                schema: "award",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "SectionId1",
                schema: "award",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "SectorId1",
                schema: "award",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "UnitId1",
                schema: "award",
                table: "Employee");

            migrationBuilder.AlterColumn<long>(
                name: "UnitId",
                schema: "award",
                table: "Employee",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "SectorId",
                schema: "award",
                table: "Employee",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<long>(
                name: "SectionId",
                schema: "award",
                table: "Employee",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "DepartmentId",
                schema: "award",
                table: "Employee",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "BranchId",
                schema: "award",
                table: "Employee",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdBranch",
                schema: "award",
                table: "Employee",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdDepartment",
                schema: "award",
                table: "Employee",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdSection",
                schema: "award",
                table: "Employee",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdSector",
                schema: "award",
                table: "Employee",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdUnit",
                schema: "award",
                table: "Employee",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employee_BranchId",
                schema: "award",
                table: "Employee",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_DepartmentId",
                schema: "award",
                table: "Employee",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_SectionId",
                schema: "award",
                table: "Employee",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_SectorId",
                schema: "award",
                table: "Employee",
                column: "SectorId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_UnitId",
                schema: "award",
                table: "Employee",
                column: "UnitId");

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
                name: "FK_Employee_Sectors_SectorId",
                schema: "award",
                table: "Employee",
                column: "SectorId",
                principalSchema: "award",
                principalTable: "Sectors",
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
    }
}
