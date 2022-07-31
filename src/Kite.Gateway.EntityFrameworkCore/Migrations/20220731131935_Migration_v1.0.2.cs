using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kite.Gateway.EntityFrameworkCore.Migrations
{
    public partial class Migration_v102 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NacosGroupName",
                table: "ServiceGovernanceConfigures",
                type: "TEXT",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NacosNamespaceId",
                table: "ServiceGovernanceConfigures",
                type: "TEXT",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NacosServer",
                table: "ServiceGovernanceConfigures",
                type: "TEXT",
                maxLength: 512,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NacosGroupName",
                table: "ServiceGovernanceConfigures");

            migrationBuilder.DropColumn(
                name: "NacosNamespaceId",
                table: "ServiceGovernanceConfigures");

            migrationBuilder.DropColumn(
                name: "NacosServer",
                table: "ServiceGovernanceConfigures");
        }
    }
}
