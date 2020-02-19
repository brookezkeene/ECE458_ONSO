using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Api.Infrastructure.Migrations
{
    public partial class AddDatacentersAndPdus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DatacenterId",
                table: "Racks",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AssetPowerPort",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Number = table.Column<int>(nullable: false),
                    AssetId = table.Column<Guid>(nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Datacenters",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 6, nullable: false),
                    Description = table.Column<string>(nullable: false),
                    HasNetworkManagedPower = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Datacenters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pdus",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    NumPorts = table.Column<int>(nullable: false),
                    LocationEnum = table.Column<int>(nullable: false),
                    RackId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pdus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pdus_Racks_RackId",
                        column: x => x.RackId,
                        principalTable: "Racks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PduPort",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Number = table.Column<int>(nullable: false),
                    PduId = table.Column<Guid>(nullable: false),
                    AssetPowerPortId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PduPort", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PduPort_AssetPowerPort_AssetPowerPortId",
                        column: x => x.AssetPowerPortId,
                        principalTable: "AssetPowerPort",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PduPort_Pdus_PduId",
                        column: x => x.PduId,
                        principalTable: "Pdus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Racks_DatacenterId",
                table: "Racks",
                column: "DatacenterId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetPowerPort_AssetId",
                table: "AssetPowerPort",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_Datacenters_Name",
                table: "Datacenters",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PduPort_AssetPowerPortId",
                table: "PduPort",
                column: "AssetPowerPortId",
                unique: true,
                filter: "[AssetPowerPortId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PduPort_PduId",
                table: "PduPort",
                column: "PduId");

            migrationBuilder.CreateIndex(
                name: "IX_Pdus_RackId",
                table: "Pdus",
                column: "RackId");

            migrationBuilder.AddForeignKey(
                name: "FK_Racks_Datacenters_DatacenterId",
                table: "Racks",
                column: "DatacenterId",
                principalTable: "Datacenters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Racks_Datacenters_DatacenterId",
                table: "Racks");

            migrationBuilder.DropTable(
                name: "Datacenters");

            migrationBuilder.DropTable(
                name: "PduPort");

            migrationBuilder.DropTable(
                name: "AssetPowerPort");

            migrationBuilder.DropTable(
                name: "Pdus");

            migrationBuilder.DropIndex(
                name: "IX_Racks_DatacenterId",
                table: "Racks");

            migrationBuilder.DropColumn(
                name: "DatacenterId",
                table: "Racks");
        }
    }
}
