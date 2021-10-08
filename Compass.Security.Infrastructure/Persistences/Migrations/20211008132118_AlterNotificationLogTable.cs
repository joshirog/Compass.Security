using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Compass.Security.Infrastructure.Persistences.Migrations
{
    public partial class AlterNotificationLogTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "blacklists",
                keyColumn: "id",
                keyValue: new Guid("647da967-883d-4de5-bfa0-eb3621e208b1"));

            migrationBuilder.AlterColumn<string>(
                name: "identifier",
                table: "notification_logs",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.InsertData(
                table: "blacklists",
                columns: new[] { "id", "created_at", "created_by", "status", "type", "updated_at", "updated_by", "Value" },
                values: new object[] { new Guid("89e87893-4fb1-4cbc-a2fb-d345784e71e5"), new DateTime(2012, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "migrations", "Active", "Password", null, null, "Secret2020$$" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "blacklists",
                keyColumn: "id",
                keyValue: new Guid("89e87893-4fb1-4cbc-a2fb-d345784e71e5"));

            migrationBuilder.AlterColumn<string>(
                name: "identifier",
                table: "notification_logs",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.InsertData(
                table: "blacklists",
                columns: new[] { "id", "created_at", "created_by", "status", "type", "updated_at", "updated_by", "Value" },
                values: new object[] { new Guid("647da967-883d-4de5-bfa0-eb3621e208b1"), new DateTime(2012, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "migrations", "Active", "Password", null, null, "Secret2020$$" });
        }
    }
}
