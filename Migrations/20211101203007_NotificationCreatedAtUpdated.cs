using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace packagesentinel.Migrations
{
    public partial class NotificationCreatedAtUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "createdAt",
                table: "Notification",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "7b3877a1-16e7-4e82-a3dd-ef6d08ba1294", "AQAAAAEAACcQAAAAEIRLy3UQFidoTiXTId5qjsB9sksReERTosnbb+ouzIYwM78Xoh6L3OPpc07CmdTEgA==" });

            migrationBuilder.UpdateData(
                table: "Device",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2021, 11, 1, 16, 30, 6, 726, DateTimeKind.Local).AddTicks(7399));

            migrationBuilder.UpdateData(
                table: "Package",
                keyColumn: "Id",
                keyValue: 1,
                column: "PlacedAt",
                value: new DateTime(2021, 11, 1, 16, 30, 6, 729, DateTimeKind.Local).AddTicks(9892));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "createdAt",
                table: "Notification",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

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
    }
}
