using Microsoft.EntityFrameworkCore.Migrations;

namespace packagesentinel.Migrations
{
    public partial class Packageupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NotificationKey",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotificationKey",
                table: "AspNetUsers");
        }
    }
}
