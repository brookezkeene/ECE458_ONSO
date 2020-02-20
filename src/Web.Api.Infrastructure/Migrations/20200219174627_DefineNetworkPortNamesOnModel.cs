using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Api.Infrastructure.Migrations
{
    public partial class DefineNetworkPortNamesOnModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PduPort_Pdus_PduId",
                table: "PduPort");

            migrationBuilder.DropForeignKey(
                name: "FK_Pdus_Racks_RackId",
                table: "Pdus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pdus",
                table: "Pdus");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AssetNetworkPort");

            migrationBuilder.RenameTable(
                name: "Pdus",
                newName: "Pdu");

            migrationBuilder.RenameIndex(
                name: "IX_Pdus_RackId",
                table: "Pdu",
                newName: "IX_Pdu_RackId");

            migrationBuilder.AddColumn<Guid>(
                name: "ModelNetworkPortId",
                table: "AssetNetworkPort",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pdu",
                table: "Pdu",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ModelNetworkPort",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 10, nullable: false),
                    ModelId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelNetworkPort", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModelNetworkPort_Models_ModelId",
                        column: x => x.ModelId,
                        principalTable: "Models",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssetNetworkPort_ModelNetworkPortId",
                table: "AssetNetworkPort",
                column: "ModelNetworkPortId");

            migrationBuilder.CreateIndex(
                name: "IX_ModelNetworkPort_ModelId",
                table: "ModelNetworkPort",
                column: "ModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssetNetworkPort_ModelNetworkPort_ModelNetworkPortId",
                table: "AssetNetworkPort",
                column: "ModelNetworkPortId",
                principalTable: "ModelNetworkPort",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pdu_Racks_RackId",
                table: "Pdu",
                column: "RackId",
                principalTable: "Racks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PduPort_Pdu_PduId",
                table: "PduPort",
                column: "PduId",
                principalTable: "Pdu",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssetNetworkPort_ModelNetworkPort_ModelNetworkPortId",
                table: "AssetNetworkPort");

            migrationBuilder.DropForeignKey(
                name: "FK_Pdu_Racks_RackId",
                table: "Pdu");

            migrationBuilder.DropForeignKey(
                name: "FK_PduPort_Pdu_PduId",
                table: "PduPort");

            migrationBuilder.DropTable(
                name: "ModelNetworkPort");

            migrationBuilder.DropIndex(
                name: "IX_AssetNetworkPort_ModelNetworkPortId",
                table: "AssetNetworkPort");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pdu",
                table: "Pdu");

            migrationBuilder.DropColumn(
                name: "ModelNetworkPortId",
                table: "AssetNetworkPort");

            migrationBuilder.RenameTable(
                name: "Pdu",
                newName: "Pdus");

            migrationBuilder.RenameIndex(
                name: "IX_Pdu_RackId",
                table: "Pdus",
                newName: "IX_Pdus_RackId");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AssetNetworkPort",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pdus",
                table: "Pdus",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PduPort_Pdus_PduId",
                table: "PduPort",
                column: "PduId",
                principalTable: "Pdus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pdus_Racks_RackId",
                table: "Pdus",
                column: "RackId",
                principalTable: "Racks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
