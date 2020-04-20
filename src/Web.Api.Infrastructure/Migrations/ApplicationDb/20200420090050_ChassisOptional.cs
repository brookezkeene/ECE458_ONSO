using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Api.Infrastructure.Migrations.ApplicationDb
{
    public partial class ChassisOptional : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<int>(
                name: "ChassisSlot",
                table: "Assets",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "ChassisId",
                table: "Assets",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_ChassisId",
                table: "Assets",
                column: "ChassisId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_Assets_ChassisId",
                table: "Assets",
                column: "ChassisId",
                principalTable: "Assets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assets_Assets_ChassisId",
                table: "Assets");

            migrationBuilder.DropIndex(
                name: "IX_Assets_ChassisId",
                table: "Assets");

            migrationBuilder.AlterColumn<int>(
                name: "ChassisSlot",
                table: "Assets",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ChassisId",
                table: "Assets",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AssetId",
                table: "Assets",
                type: "uniqueidentifier",
                nullable: true);

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
    }
}
