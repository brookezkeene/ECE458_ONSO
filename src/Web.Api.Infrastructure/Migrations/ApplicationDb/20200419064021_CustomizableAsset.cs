using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Api.Infrastructure.Migrations.ApplicationDb
{
    public partial class CustomizableAsset : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BladePowerState",
                table: "Assets");

            migrationBuilder.AddColumn<string>(
                name: "CustomCpu",
                table: "Assets",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomDisplayColor",
                table: "Assets",
                maxLength: 7,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CustomMemory",
                table: "Assets",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomStorage",
                table: "Assets",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomCpu",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "CustomDisplayColor",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "CustomMemory",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "CustomStorage",
                table: "Assets");

            migrationBuilder.AddColumn<bool>(
                name: "BladePowerState",
                table: "Assets",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
