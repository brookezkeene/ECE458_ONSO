using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Api.Infrastructure.Migrations.ApplicationDb
{
    public partial class AssetBladeModification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AssetId",
                table: "Assets",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "BladePowerState",
                table: "Assets",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ChassisId",
                table: "Assets",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "ChassisSlot",
                table: "Assets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Assets_AssetId",
                table: "Assets",
                column: "AssetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_Assets_AssetId",
                table: "Assets",
                column: "AssetId",
                principalTable: "Assets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assets_Assets_AssetId",
                table: "Assets");

            migrationBuilder.DropIndex(
                name: "IX_Assets_AssetId",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "AssetId",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "BladePowerState",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "ChassisId",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "ChassisSlot",
                table: "Assets");
        }
    }
}
