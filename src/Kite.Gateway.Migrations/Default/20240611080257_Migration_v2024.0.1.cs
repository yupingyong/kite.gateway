using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kite.Gateway.Migrations.Default
{
    /// <inheritdoc />
    public partial class Migration_v202401 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuthAccount",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AccountName = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    NickName = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    HeadUrl = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true),
                    Password = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    IsEnable = table.Column<bool>(type: "INTEGER", nullable: false),
                    RoleId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Updated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthAccount", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuthAuthorize",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    RoleId = table.Column<Guid>(type: "TEXT", nullable: false),
                    MenuId = table.Column<Guid>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthAuthorize", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuthMenu",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    MenuName = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    AuthCode = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    MenuType = table.Column<int>(type: "INTEGER", nullable: false),
                    ParentId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Updated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthMenu", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuthRole",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    RoleName = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Updated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthRole", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthAccount");

            migrationBuilder.DropTable(
                name: "AuthAuthorize");

            migrationBuilder.DropTable(
                name: "AuthMenu");

            migrationBuilder.DropTable(
                name: "AuthRole");
        }
    }
}
