using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Api.Infrastructure.Migrations.ApplicationDb
{
    public partial class PowerConnectionCascadeDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PowerPort_PowerConnections_PowerConnectionId",
                table: "PowerPort");

            migrationBuilder.DropForeignKey(
                name: "FK_AssetNetworkPort_NetworkConnections_NetworkConnectionId",
                table: "AssetNetworkPort");

            migrationBuilder.AddForeignKey(
                name: "FK_PowerPort_PowerConnections_PowerConnectionId",
                table: "PowerPort",
                column: "PowerConnectionId",
                principalTable: "PowerConnections",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_AssetNetworkPort_NetworkConnections_NetworkConnectionId",
                table: "AssetNetworkPort",
                column: "NetworkConnectionId",
                principalTable: "NetworkConnections",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
