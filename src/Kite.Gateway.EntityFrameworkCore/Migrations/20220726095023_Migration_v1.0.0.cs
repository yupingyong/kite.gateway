using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kite.Gateway.EntityFrameworkCore.Migrations
{
    public partial class Migration_v100 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Administrators",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AdminName = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    Password = table.Column<string>(type: "TEXT", maxLength: 64, nullable: true),
                    NickName = table.Column<string>(type: "TEXT", maxLength: 64, nullable: true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Updated = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administrators", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuthenticationConfigures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UseState = table.Column<bool>(type: "INTEGER", nullable: false),
                    Issuer = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true),
                    Audience = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true),
                    ClockSkew = table.Column<int>(type: "INTEGER", nullable: false),
                    ValidateIssuerSigningKey = table.Column<bool>(type: "INTEGER", nullable: false),
                    ValidateIssuer = table.Column<bool>(type: "INTEGER", nullable: false),
                    ValidateAudience = table.Column<bool>(type: "INTEGER", nullable: false),
                    ValidateLifetime = table.Column<bool>(type: "INTEGER", nullable: false),
                    RequireExpirationTime = table.Column<bool>(type: "INTEGER", nullable: false),
                    UseSSL = table.Column<bool>(type: "INTEGER", nullable: false),
                    SecurityKeyStr = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true),
                    CertificateFile = table.Column<string>(type: "TEXT", nullable: true),
                    CertificateFileName = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    CertificatePassword = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthenticationConfigures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClusterDestinations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ClusterId = table.Column<Guid>(type: "TEXT", nullable: false),
                    DestinationName = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    DestinationAddress = table.Column<string>(type: "TEXT", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClusterDestinations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClusterHealthChecks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ClusterId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Enabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    Interval = table.Column<int>(type: "INTEGER", nullable: false),
                    Timeout = table.Column<int>(type: "INTEGER", nullable: false),
                    Policy = table.Column<string>(type: "TEXT", maxLength: 64, nullable: true),
                    Path = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClusterHealthChecks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clusters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    RouteId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ClusterName = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    LoadBalancingPolicy = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    ServiceGovernanceType = table.Column<int>(type: "INTEGER", nullable: false),
                    ServiceGovernanceName = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clusters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Middlewares",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 64, nullable: true),
                    Server = table.Column<string>(type: "TEXT", maxLength: 1024, nullable: true),
                    SignalType = table.Column<int>(type: "INTEGER", nullable: false),
                    UseState = table.Column<bool>(type: "INTEGER", nullable: false),
                    ExecWeight = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1024, nullable: true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Updated = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Middlewares", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Nodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    NodeName = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    Description = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true),
                    Server = table.Column<string>(type: "TEXT", maxLength: 1024, nullable: true),
                    Token = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Updated = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Routes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    RouteId = table.Column<string>(type: "TEXT", maxLength: 64, nullable: true),
                    RouteName = table.Column<string>(type: "TEXT", maxLength: 64, nullable: true),
                    Description = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true),
                    UseState = table.Column<bool>(type: "INTEGER", nullable: false),
                    RouteMatchPath = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Updated = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RouteTransforms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    RouteId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TransformsName = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    TransformsValue = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouteTransforms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceGovernanceConfigures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ConsulServer = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true),
                    ConsulDatacenter = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    ConsulToken = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceGovernanceConfigures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Whitelists",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    RouteId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 64, nullable: true),
                    FilterType = table.Column<int>(type: "INTEGER", nullable: false),
                    FilterText = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    RequestMethod = table.Column<string>(type: "TEXT", maxLength: 64, nullable: true),
                    UseState = table.Column<bool>(type: "INTEGER", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Whitelists", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Administrators");

            migrationBuilder.DropTable(
                name: "AuthenticationConfigures");

            migrationBuilder.DropTable(
                name: "ClusterDestinations");

            migrationBuilder.DropTable(
                name: "ClusterHealthChecks");

            migrationBuilder.DropTable(
                name: "Clusters");

            migrationBuilder.DropTable(
                name: "Middlewares");

            migrationBuilder.DropTable(
                name: "Nodes");

            migrationBuilder.DropTable(
                name: "Routes");

            migrationBuilder.DropTable(
                name: "RouteTransforms");

            migrationBuilder.DropTable(
                name: "ServiceGovernanceConfigures");

            migrationBuilder.DropTable(
                name: "Whitelists");
        }
    }
}
