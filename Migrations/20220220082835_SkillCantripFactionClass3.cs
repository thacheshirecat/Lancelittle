using Microsoft.EntityFrameworkCore.Migrations;

namespace Lancelittle.Migrations
{
    public partial class SkillCantripFactionClass3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Cost",
                table: "Skills",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cost",
                table: "Skills");
        }
    }
}
