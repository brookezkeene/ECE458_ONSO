using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Api.Infrastructure.Migrations.ApplicationDb
{
    public partial class SymmetricPowerConnections : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PduPort_AssetPowerPort_AssetPowerPortId",
                table: "PduPort");

            migrationBuilder.DropForeignKey(
                name: "FK_PduPort_Pdu_PduId",
                table: "PduPort");

            migrationBuilder.DropTable(
                name: "AssetPowerPort");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PduPort",
                table: "PduPort");

            migrationBuilder.DropIndex(
                name: "IX_PduPort_AssetPowerPortId",
                table: "PduPort");

            migrationBuilder.DropColumn(
                name: "AssetPowerPortId",
                table: "PduPort");

            migrationBuilder.RenameTable(
                name: "PduPort",
                newName: "PowerPort");

            migrationBuilder.RenameIndex(
                name: "IX_PduPort_PduId",
                table: "PowerPort",
                newName: "IX_PowerPort_PduId");

            migrationBuilder.AlterColumn<Guid>(
                name: "PduId",
                table: "PowerPort",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "AssetId",
                table: "PowerPort",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PowerConnectionId1",
                table: "PowerPort",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PduPort_PowerConnectionId1",
                table: "PowerPort",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "PowerPort",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "PowerConnectionId",
                table: "PowerPort",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PowerPort",
                table: "PowerPort",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "PowerConnections",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PowerConnections", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PowerPort_AssetId",
                table: "PowerPort",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_PowerPort_PowerConnectionId1",
                table: "PowerPort",
                column: "PowerConnectionId1");

            migrationBuilder.CreateIndex(
                name: "IX_PowerPort_PduPort_PowerConnectionId1",
                table: "PowerPort",
                column: "PduPort_PowerConnectionId1");

            migrationBuilder.CreateIndex(
                name: "IX_PowerPort_PowerConnectionId",
                table: "PowerPort",
                column: "PowerConnectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_PowerPort_Assets_AssetId",
                table: "PowerPort",
                column: "AssetId",
                principalTable: "Assets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PowerPort_PowerConnections_PowerConnectionId1",
                table: "PowerPort",
                column: "PowerConnectionId1",
                principalTable: "PowerConnections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PowerPort_Pdu_PduId",
                table: "PowerPort",
                column: "PduId",
                principalTable: "Pdu",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PowerPort_PowerConnections_PduPort_PowerConnectionId1",
                table: "PowerPort",
                column: "PduPort_PowerConnectionId1",
                principalTable: "PowerConnections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PowerPort_PowerConnections_PowerConnectionId",
                table: "PowerPort",
                column: "PowerConnectionId",
                principalTable: "PowerConnections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PowerPort_Assets_AssetId",
                table: "PowerPort");

            migrationBuilder.DropForeignKey(
                name: "FK_PowerPort_PowerConnections_PowerConnectionId1",
                table: "PowerPort");

            migrationBuilder.DropForeignKey(
                name: "FK_PowerPort_Pdu_PduId",
                table: "PowerPort");

            migrationBuilder.DropForeignKey(
                name: "FK_PowerPort_PowerConnections_PduPort_PowerConnectionId1",
                table: "PowerPort");

            migrationBuilder.DropForeignKey(
                name: "FK_PowerPort_PowerConnections_PowerConnectionId",
                table: "PowerPort");

            migrationBuilder.DropTable(
                name: "PowerConnections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PowerPort",
                table: "PowerPort");

            migrationBuilder.DropIndex(
                name: "IX_PowerPort_AssetId",
                table: "PowerPort");

            migrationBuilder.DropIndex(
                name: "IX_PowerPort_PowerConnectionId1",
                table: "PowerPort");

            migrationBuilder.DropIndex(
                name: "IX_PowerPort_PduPort_PowerConnectionId1",
                table: "PowerPort");

            migrationBuilder.DropIndex(
                name: "IX_PowerPort_PowerConnectionId",
                table: "PowerPort");

            migrationBuilder.DropColumn(
                name: "AssetId",
                table: "PowerPort");

            migrationBuilder.DropColumn(
                name: "PowerConnectionId1",
                table: "PowerPort");

            migrationBuilder.DropColumn(
                name: "PduPort_PowerConnectionId1",
                table: "PowerPort");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "PowerPort");

            migrationBuilder.DropColumn(
                name: "PowerConnectionId",
                table: "PowerPort");

            migrationBuilder.RenameTable(
                name: "PowerPort",
                newName: "PduPort");

            migrationBuilder.RenameIndex(
                name: "IX_PowerPort_PduId",
                table: "PduPort",
                newName: "IX_PduPort_PduId");

            migrationBuilder.AlterColumn<Guid>(
                name: "PduId",
                table: "PduPort",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AssetPowerPortId",
                table: "PduPort",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PduPort",
                table: "PduPort",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AssetPowerPort",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    PduPortId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetPowerPort", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssetPowerPort_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PduPort_AssetPowerPortId",
                table: "PduPort",
                column: "AssetPowerPortId",
                unique: true,
                filter: "[AssetPowerPortId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AssetPowerPort_AssetId",
                table: "AssetPowerPort",
                column: "AssetId");

            migrationBuilder.AddForeignKey(
                name: "FK_PduPort_AssetPowerPort_AssetPowerPortId",
                table: "PduPort",
                column: "AssetPowerPortId",
                principalTable: "AssetPowerPort",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PduPort_Pdu_PduId",
                table: "PduPort",
                column: "PduId",
                principalTable: "Pdu",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
