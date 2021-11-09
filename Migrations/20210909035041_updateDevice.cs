using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace packagesentinel.Migrations
{
    public partial class updateDevice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PickupAcknowledged",
                table: "Package",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "SyncedAt",
                table: "Device",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "7e6203e6-0966-4c39-948b-241d70cf6619", "AQAAAAEAACcQAAAAEH6QFG57i2Cbn/4zmZFmDNjdxmUBCCQF8bhz9949VbnHjmjGZfkYfZDwPWHsoqyPzw==" });

            migrationBuilder.UpdateData(
                table: "Device",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2021, 9, 8, 23, 50, 40, 809, DateTimeKind.Local).AddTicks(2596));

            migrationBuilder.UpdateData(
                table: "Package",
                keyColumn: "Id",
                keyValue: 1,
                column: "PlacedAt",
                value: new DateTime(2021, 9, 8, 23, 50, 40, 814, DateTimeKind.Local).AddTicks(6280));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PickupAcknowledged",
                table: "Package");

            migrationBuilder.DropColumn(
                name: "SyncedAt",
                table: "Device");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "27ff3951-20bc-4427-bc3b-2f755f6b2ef3", "AQAAAAEAACcQAAAAEDWqN0y8ubLweilAGVnsWZDMl9Tx+whLdXc8N+eSih7i0/iyk7sgGYc2PiCcjA+89g==" });

            migrationBuilder.UpdateData(
                table: "Device",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2021, 8, 5, 0, 16, 48, 734, DateTimeKind.Local).AddTicks(7217));

            migrationBuilder.UpdateData(
                table: "Package",
                keyColumn: "Id",
                keyValue: 1,
                column: "PlacedAt",
                value: new DateTime(2021, 8, 5, 0, 16, 48, 737, DateTimeKind.Local).AddTicks(4129));
        }
    }
}
