using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Api.Infrastructure.Migrations.ApplicationDb
{
    public partial class SymmetricNetworkConnections : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssetNetworkPort_AssetNetworkPort_ConnectedPortId",
                table: "AssetNetworkPort");

            migrationBuilder.DropIndex(
                name: "IX_AssetNetworkPort_ConnectedPortId",
                table: "AssetNetworkPort");

            migrationBuilder.DropColumn(
                name: "ConnectedPortId",
                table: "AssetNetworkPort");

            migrationBuilder.AddColumn<Guid>(
                name: "NetworkConnectionId",
                table: "AssetNetworkPort",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "NetworkConnections",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NetworkConnections", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssetNetworkPort_NetworkConnectionId",
                table: "AssetNetworkPort",
                column: "NetworkConnectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssetNetworkPort_NetworkConnections_NetworkConnectionId",
                table: "AssetNetworkPort",
                column: "NetworkConnectionId",
                principalTable: "NetworkConnections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssetNetworkPort_NetworkConnections_NetworkConnectionId",
                table: "AssetNetworkPort");

            migrationBuilder.DropTable(
                name: "NetworkConnections");

            migrationBuilder.DropIndex(
                name: "IX_AssetNetworkPort_NetworkConnectionId",
                table: "AssetNetworkPort");

            migrationBuilder.DropColumn(
                name: "NetworkConnectionId",
                table: "AssetNetworkPort");

            migrationBuilder.AddColumn<Guid>(
                name: "ConnectedPortId",
                table: "AssetNetworkPort",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssetNetworkPort_ConnectedPortId",
                table: "AssetNetworkPort",
                column: "ConnectedPortId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssetNetworkPort_AssetNetworkPort_ConnectedPortId",
                table: "AssetNetworkPort",
                column: "ConnectedPortId",
                principalTable: "AssetNetworkPort",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
