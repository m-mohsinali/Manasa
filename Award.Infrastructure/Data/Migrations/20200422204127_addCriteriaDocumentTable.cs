using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Award.Infrastructure.Data.Migrations
{
    public partial class addCriteriaDocumentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
             
      
 

         

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
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "SectorId",
                schema: "award",
                table: "Employee",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "SectionId",
                schema: "award",
                table: "Employee",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "DepartmentId",
                schema: "award",
                table: "Employee",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "BranchId",
                schema: "award",
                table: "Employee",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
 

            migrationBuilder.CreateTable(
                name: "CriteriaDocuments",
                schema: "award",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    QSMEntryId = table.Column<long>(nullable: false),
                    SupportingDocumentPath = table.Column<string>(nullable: true),
                    CurrentStatusId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CriteriaDocuments", x => x.Id);
                });

            
 

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
                onDelete: ReferentialAction.Cascade);

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

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeTeams_Teams_TeamId",
                schema: "award",
                table: "EmployeeTeams");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeTeams_Users_UserId",
                schema: "award",
                table: "EmployeeTeams");

            migrationBuilder.DropTable(
                name: "CriteriaDocuments",
                schema: "award");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeTeams",
                schema: "award",
                table: "EmployeeTeams");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeTeams_TeamId",
                schema: "award",
                table: "EmployeeTeams");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeTeams_UserId_TeamId",
                schema: "award",
                table: "EmployeeTeams");

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
                name: "TeamId",
                schema: "award",
                table: "EmployeeTeams");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "award",
                table: "EmployeeTeams");

            migrationBuilder.AddColumn<long>(
                name: "IdEmployee",
                schema: "award",
                table: "EmployeeTeams",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "IdTeam",
                schema: "award",
                table: "EmployeeTeams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "TeamsId",
                schema: "award",
                table: "EmployeeTeams",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UnitId",
                schema: "award",
                table: "Employee",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SectorId",
                schema: "award",
                table: "Employee",
                type: "int",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<int>(
                name: "SectionId",
                schema: "award",
                table: "Employee",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                schema: "award",
                table: "Employee",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BranchId",
                schema: "award",
                table: "Employee",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "BranchId1",
                schema: "award",
                table: "Employee",
                type: "bigint",
                nullable: true);

             
 

            migrationBuilder.AddColumn<long>(
                name: "SectorId1",
                schema: "award",
                table: "Employee",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UnitId1",
                schema: "award",
                table: "Employee",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeTeams",
                schema: "award",
                table: "EmployeeTeams",
                columns: new[] { "IdEmployee", "IdTeam" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeTeams_TeamsId",
                schema: "award",
                table: "EmployeeTeams",
                column: "TeamsId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_BranchId1",
                schema: "award",
                table: "Employee",
                column: "BranchId1");

            
 

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

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeTeams_Teams_TeamsId",
                schema: "award",
                table: "EmployeeTeams",
                column: "TeamsId",
                principalSchema: "award",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
