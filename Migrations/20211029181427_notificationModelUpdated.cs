using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace packagesentinel.Migrations
{
    public partial class notificationModelUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "createdAt",
                table: "Notification",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "95c1a1fb-cc1d-4865-b5dc-2e6b7e67eded", "AQAAAAEAACcQAAAAEHlPScISxLy7Ska29hM+RRI8wW3Btb0qT6EUwPNaJi9SNpSBqHlfKoSzNMb10C+c0Q==" });

            migrationBuilder.UpdateData(
                table: "Device",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2021, 10, 29, 14, 14, 26, 325, DateTimeKind.Local).AddTicks(1662));

            migrationBuilder.UpdateData(
                table: "Package",
                keyColumn: "Id",
                keyValue: 1,
                column: "PlacedAt",
                value: new DateTime(2021, 10, 29, 14, 14, 26, 327, DateTimeKind.Local).AddTicks(9314));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "createdAt",
                table: "Notification");

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
    }
}
