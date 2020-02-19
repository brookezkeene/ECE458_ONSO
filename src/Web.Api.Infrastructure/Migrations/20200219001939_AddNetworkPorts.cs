using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Api.Infrastructure.Migrations
{
    public partial class AddNetworkPorts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssetNetworkPort",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 10, nullable: false),
                    MacAddress = table.Column<string>(maxLength: 17, nullable: true),
                    AssetId = table.Column<Guid>(nullable: false),
                    ConnectedPortId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetNetworkPort", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssetNetworkPort_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssetNetworkPort_AssetNetworkPort_ConnectedPortId",
                        column: x => x.ConnectedPortId,
                        principalTable: "AssetNetworkPort",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssetNetworkPort_AssetId",
                table: "AssetNetworkPort",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetNetworkPort_ConnectedPortId",
                table: "AssetNetworkPort",
                column: "ConnectedPortId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssetNetworkPort");
        }
    }
}
