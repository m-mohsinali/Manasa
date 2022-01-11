using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Award.Infrastructure.Data.Migrations
{
    public partial class add_employee_details : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    NameAr = table.Column<string>(maxLength: 300, nullable: true),
                    NameEn = table.Column<string>(maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    NameAr = table.Column<string>(maxLength: 300, nullable: true),
                    NameEn = table.Column<string>(maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ManasaEmployees",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GRP = table.Column<int>(nullable: true),
                    NameEN = table.Column<string>(nullable: true),
                    NameAR = table.Column<string>(nullable: true),
                    RankAR = table.Column<string>(nullable: true),
                    RankEN = table.Column<string>(nullable: true),
                    JobTitleEN = table.Column<string>(nullable: true),
                    JobTitleAR = table.Column<string>(nullable: true),
                    ClassEN = table.Column<string>(nullable: true),
                    ClassAR = table.Column<string>(nullable: true),
                    SectorNameAR = table.Column<string>(nullable: true),
                    SectorNameEN = table.Column<string>(nullable: true),
                    DeptNameAR = table.Column<string>(nullable: true),
                    DeptNameEN = table.Column<string>(nullable: true),
                    SectionNameAR = table.Column<string>(nullable: true),
                    SectionNameEN = table.Column<string>(nullable: true),
                    UnitNameEN = table.Column<string>(nullable: true),
                    UnitNameAR = table.Column<string>(nullable: true),
                    BranchNameEN = table.Column<string>(nullable: true),
                    BranchNameAR = table.Column<string>(nullable: true),
                    EmployeePhotoURL = table.Column<string>(nullable: true),
                    LastUpdaeDate = table.Column<DateTime>(nullable: true),
                    UserDomain = table.Column<string>(nullable: true),
                    SexEN = table.Column<string>(nullable: true),
                    SexAR = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManasaEmployees", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Sections",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    NameAr = table.Column<string>(maxLength: 300, nullable: true),
                    NameEn = table.Column<string>(maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sectors",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    NameEn = table.Column<string>(maxLength: 300, nullable: true),
                    NameAr = table.Column<string>(maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sectors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    NameAr = table.Column<string>(maxLength: 300, nullable: true),
                    NameEn = table.Column<string>(maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CategoriesSectors",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    IdSector = table.Column<int>(nullable: false),
                    IdCategory = table.Column<long>(nullable: false),
                    IsNotified = table.Column<bool>(nullable: true),
                    IsAccepted = table.Column<bool>(nullable: true),
                    AssignedAt = table.Column<DateTime>(nullable: true),
                    CategoryId = table.Column<long>(nullable: true),
                    SectorId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriesSectors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoriesSectors_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CategoriesSectors_Sectors_SectorId",
                        column: x => x.SectorId,
                        principalTable: "Sectors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CategoryTeams",
                columns: table => new
                {
                    IdTeams = table.Column<int>(nullable: false),
                    IdCategory = table.Column<long>(nullable: false),
                    Id = table.Column<long>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    TeamsId = table.Column<long>(nullable: true),
                    CategoryId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryTeams", x => new { x.IdTeams, x.IdCategory });
                    table.ForeignKey(
                        name: "FK_CategoryTeams_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CategoryTeams_Teams_TeamsId",
                        column: x => x.TeamsId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    IdSector = table.Column<int>(nullable: false),
                    IdDepartment = table.Column<int>(nullable: true),
                    IdSection = table.Column<int>(nullable: true),
                    IdBranch = table.Column<int>(nullable: true),
                    IdUnit = table.Column<int>(nullable: true),
                    NameAr = table.Column<string>(maxLength: 300, nullable: true),
                    NameEn = table.Column<string>(maxLength: 300, nullable: true),
                    SexEn = table.Column<string>(maxLength: 12, nullable: true),
                    SexAr = table.Column<string>(maxLength: 12, nullable: true),
                    Grp = table.Column<string>(maxLength: 50, nullable: false),
                    UserDomain = table.Column<string>(maxLength: 50, nullable: false),
                    RankAr = table.Column<string>(maxLength: 300, nullable: true),
                    RankEn = table.Column<string>(maxLength: 300, nullable: true),
                    JobAr = table.Column<string>(maxLength: 300, nullable: true),
                    JobEn = table.Column<string>(maxLength: 300, nullable: true),
                    ClassAr = table.Column<string>(maxLength: 300, nullable: true),
                    ClassEn = table.Column<string>(maxLength: 300, nullable: true),
                    EmployeePhotoUrl = table.Column<string>(maxLength: 500, nullable: true),
                    LastUpdateAt = table.Column<DateTime>(nullable: true),
                    SectorId = table.Column<long>(nullable: true),
                    DepartmentId = table.Column<long>(nullable: true),
                    SectionId = table.Column<long>(nullable: true),
                    BranchId = table.Column<long>(nullable: true),
                    UnitId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employee_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employee_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employee_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employee_Sectors_SectorId",
                        column: x => x.SectorId,
                        principalTable: "Sectors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employee_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CategoryEmployee",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    IdEmployee = table.Column<long>(nullable: false),
                    IdCategory = table.Column<long>(nullable: false),
                    NotifiedTries = table.Column<short>(nullable: false),
                    AssignedAt = table.Column<DateTime>(nullable: false),
                    IdCategoryNavigationId = table.Column<long>(nullable: true),
                    IdEmployeeNavigationId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryEmployee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoryEmployee_Categories_IdCategoryNavigationId",
                        column: x => x.IdCategoryNavigationId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CategoryEmployee_Employee_IdEmployeeNavigationId",
                        column: x => x.IdEmployeeNavigationId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeTeams",
                columns: table => new
                {
                    IdEmployee = table.Column<long>(nullable: false),
                    IdTeam = table.Column<int>(nullable: false),
                    Id = table.Column<long>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    EmployeeId = table.Column<long>(nullable: true),
                    TeamsId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeTeams", x => new { x.IdEmployee, x.IdTeam });
                    table.ForeignKey(
                        name: "FK_EmployeeTeams_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeTeams_Teams_TeamsId",
                        column: x => x.TeamsId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoriesSectors_CategoryId",
                table: "CategoriesSectors",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoriesSectors_SectorId",
                table: "CategoriesSectors",
                column: "SectorId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryEmployee_IdCategoryNavigationId",
                table: "CategoryEmployee",
                column: "IdCategoryNavigationId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryEmployee_IdEmployeeNavigationId",
                table: "CategoryEmployee",
                column: "IdEmployeeNavigationId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryTeams_CategoryId",
                table: "CategoryTeams",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryTeams_TeamsId",
                table: "CategoryTeams",
                column: "TeamsId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_BranchId",
                table: "Employee",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_DepartmentId",
                table: "Employee",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_SectionId",
                table: "Employee",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_SectorId",
                table: "Employee",
                column: "SectorId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_UnitId",
                table: "Employee",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_UserDomain",
                table: "Employee",
                column: "UserDomain",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeTeams_EmployeeId",
                table: "EmployeeTeams",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeTeams_TeamsId",
                table: "EmployeeTeams",
                column: "TeamsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoriesSectors");

            migrationBuilder.DropTable(
                name: "CategoryEmployee");

            migrationBuilder.DropTable(
                name: "CategoryTeams");

            migrationBuilder.DropTable(
                name: "EmployeeTeams");

            migrationBuilder.DropTable(
                name: "ManasaEmployees");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Branches");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Sections");

            migrationBuilder.DropTable(
                name: "Sectors");

            migrationBuilder.DropTable(
                name: "Units");
        }
    }
}
