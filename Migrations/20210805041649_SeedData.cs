using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace packagesentinel.Migrations
{
    public partial class SeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Device",
                columns: new[] { "Id", "CreatedAt", "Note", "Number", "Status", "Version" },
                values: new object[] { 1, new DateTime(2021, 8, 5, 0, 16, 48, 734, DateTimeKind.Local).AddTicks(7217), "created by seeding", "123", null, "v1.9" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DeviceId", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "NotificationKey", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "00000000-0000-0000-0000-000000000000", 0, "27ff3951-20bc-4427-bc3b-2f755f6b2ef3", 1, "admin", false, null, false, null, "admin", "admin", null, "AQAAAAEAACcQAAAAEDWqN0y8ubLweilAGVnsWZDMl9Tx+whLdXc8N+eSih7i0/iyk7sgGYc2PiCcjA+89g==", null, false, "00000000-0000-0000-0000-000000000000", false, "admin" });

            migrationBuilder.InsertData(
                table: "Package",
                columns: new[] { "Id", "Confidence", "DeviceId", "PickedUpAt", "PickedUpBy", "PlacedAt" },
                values: new object[] { 1, 0.0, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2021, 8, 5, 0, 16, 48, 737, DateTimeKind.Local).AddTicks(4129) });

            migrationBuilder.InsertData(
                table: "Setting",
                columns: new[] { "Id", "DeviceId", "IsAlarmOn", "IsNotificationOn", "IsSleep" },
                values: new object[] { 1, 1, false, false, false });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000");

            migrationBuilder.DeleteData(
                table: "Package",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Setting",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Device",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
