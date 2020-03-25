using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Api.Infrastructure.Migrations
{
    public partial class AssetLastUpdatedDateAndDecommissionedAssetNewColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "DecommissionedAssets");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDecommissioned",
                table: "DecommissionedAssets",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "OwnerName",
                table: "DecommissionedAssets",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RackPosition",
                table: "DecommissionedAssets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedDate",
                table: "Assets",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateDecommissioned",
                table: "DecommissionedAssets");

            migrationBuilder.DropColumn(
                name: "OwnerName",
                table: "DecommissionedAssets");

            migrationBuilder.DropColumn(
                name: "RackPosition",
                table: "DecommissionedAssets");

            migrationBuilder.DropColumn(
                name: "LastUpdatedDate",
                table: "Assets");

            migrationBuilder.AddColumn<string>(
                name: "Date",
                table: "DecommissionedAssets",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
