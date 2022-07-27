using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kite.Gateway.EntityFrameworkCore.Migrations
{
    public partial class Migration_v101 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Token",
                table: "Nodes",
                newName: "AccessToken");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AccessToken",
                table: "Nodes",
                newName: "Token");
        }
    }
}
