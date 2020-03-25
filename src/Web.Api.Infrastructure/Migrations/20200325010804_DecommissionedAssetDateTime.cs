using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Api.Infrastructure.Migrations
{
    public partial class DecommissionedAssetDateTime : Migration
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateDecommissioned",
                table: "DecommissionedAssets");

            migrationBuilder.AddColumn<string>(
                name: "Date",
                table: "DecommissionedAssets",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
