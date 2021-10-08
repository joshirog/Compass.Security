using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Compass.Security.Infrastructure.Persistences.Migrations
{
    public partial class CreateUserConfigurationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "blacklists",
                keyColumn: "id",
                keyValue: new Guid("723bc55b-d08e-45a0-b815-ae7b8afb5224"));

            migrationBuilder.CreateTable(
                name: "notification_logs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", maxLength: 36, nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    identifier = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notification_logs", x => x.id);
                    table.ForeignKey(
                        name: "FK_notification_logs_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_notifications",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", maxLength: 36, nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    sms_counter = table.Column<int>(type: "integer", maxLength: 10, nullable: false),
                    email_counter = table.Column<int>(type: "integer", maxLength: 50, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_notifications", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_notifications_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "blacklists",
                columns: new[] { "id", "created_at", "created_by", "status", "type", "updated_at", "updated_by", "Value" },
                values: new object[] { new Guid("647da967-883d-4de5-bfa0-eb3621e208b1"), new DateTime(2012, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "migrations", "Active", "Password", null, null, "Secret2020$$" });

            migrationBuilder.CreateIndex(
                name: "IX_notification_logs_user_id",
                table: "notification_logs",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_notifications_user_id",
                table: "user_notifications",
                column: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "notification_logs");

            migrationBuilder.DropTable(
                name: "user_notifications");

            migrationBuilder.DeleteData(
                table: "blacklists",
                keyColumn: "id",
                keyValue: new Guid("647da967-883d-4de5-bfa0-eb3621e208b1"));

            migrationBuilder.InsertData(
                table: "blacklists",
                columns: new[] { "id", "created_at", "created_by", "status", "type", "updated_at", "updated_by", "Value" },
                values: new object[] { new Guid("723bc55b-d08e-45a0-b815-ae7b8afb5224"), new DateTime(2012, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "migrations", "Active", "Password", null, null, "12345678" });
        }
    }
}
