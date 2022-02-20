using Microsoft.EntityFrameworkCore.Migrations;

namespace Lancelittle.Migrations
{
    public partial class AuthLevel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AuthLevel",
                table: "Users",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthLevel",
                table: "Users");
        }
    }
}
