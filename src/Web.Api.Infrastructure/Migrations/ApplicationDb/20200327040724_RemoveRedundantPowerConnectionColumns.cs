using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Api.Infrastructure.Migrations.ApplicationDb
{
    public partial class RemoveRedundantPowerConnectionColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PowerPort_PowerConnections_PowerConnectionId1",
                table: "PowerPort");

            migrationBuilder.DropForeignKey(
                name: "FK_PowerPort_PowerConnections_PduPort_PowerConnectionId1",
                table: "PowerPort");

            migrationBuilder.DropIndex(
                name: "IX_PowerPort_PowerConnectionId1",
                table: "PowerPort");

            migrationBuilder.DropIndex(
                name: "IX_PowerPort_PduPort_PowerConnectionId1",
                table: "PowerPort");

            migrationBuilder.DropColumn(
                name: "PowerConnectionId1",
                table: "PowerPort");

            migrationBuilder.DropColumn(
                name: "PduPort_PowerConnectionId1",
                table: "PowerPort");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PowerConnectionId1",
                table: "PowerPort",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PduPort_PowerConnectionId1",
                table: "PowerPort",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PowerPort_PowerConnectionId1",
                table: "PowerPort",
                column: "PowerConnectionId1");

            migrationBuilder.CreateIndex(
                name: "IX_PowerPort_PduPort_PowerConnectionId1",
                table: "PowerPort",
                column: "PduPort_PowerConnectionId1");

            migrationBuilder.AddForeignKey(
                name: "FK_PowerPort_PowerConnections_PowerConnectionId1",
                table: "PowerPort",
                column: "PowerConnectionId1",
                principalTable: "PowerConnections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PowerPort_PowerConnections_PduPort_PowerConnectionId1",
                table: "PowerPort",
                column: "PduPort_PowerConnectionId1",
                principalTable: "PowerConnections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
