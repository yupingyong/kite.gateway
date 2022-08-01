using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kite.Gateway.EntityFrameworkCore.Migrations
{
    public partial class Migration_v103 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilterType",
                table: "Whitelists");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FilterType",
                table: "Whitelists",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
