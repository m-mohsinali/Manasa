using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Award.Infrastructure.Data.Migrations
{
    public partial class NotificationTablesModify : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                schema: "award",
                table: "NotificationTypes");

            migrationBuilder.AddColumn<string>(
                name: "TypeAr",
                schema: "award",
                table: "NotificationTypes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TypeEn",
                schema: "award",
                table: "NotificationTypes",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckDate",
                schema: "award",
                table: "Notifications",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeAr",
                schema: "award",
                table: "NotificationTypes");

            migrationBuilder.DropColumn(
                name: "TypeEn",
                schema: "award",
                table: "NotificationTypes");

            migrationBuilder.DropColumn(
                name: "CheckDate",
                schema: "award",
                table: "Notifications");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                schema: "award",
                table: "NotificationTypes",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
