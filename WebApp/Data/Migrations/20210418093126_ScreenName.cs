using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApp.Data.Migrations
{
    public partial class ScreenName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ScreenName",
                table: "AspNetUsers",
                type: "varchar(95) CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ScreenName",
                table: "AspNetUsers",
                column: "ScreenName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ScreenName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ScreenName",
                table: "AspNetUsers");
        }
    }
}
