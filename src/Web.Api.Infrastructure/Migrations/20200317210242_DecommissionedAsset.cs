using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Api.Infrastructure.Migrations
{
    public partial class DecommissionedAsset : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DecommissionedAssets",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Hostname = table.Column<string>(nullable: true),
                    Datacenter = table.Column<string>(nullable: true),
                    ModelName = table.Column<string>(nullable: true),
                    ModelNumber = table.Column<string>(nullable: true),
                    Decommissioner = table.Column<string>(nullable: true),
                    Date = table.Column<string>(nullable: true),
                    Rack = table.Column<string>(nullable: true),
                    Data = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DecommissionedAssets", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DecommissionedAssets");
        }
    }
}
