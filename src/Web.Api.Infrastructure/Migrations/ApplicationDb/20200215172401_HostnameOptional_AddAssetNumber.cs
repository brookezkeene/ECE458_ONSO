using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Api.Infrastructure.Migrations.ApplicationDb
{
    public partial class HostnameOptional_AddAssetNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Instances_Hostname",
                table: "Instances");

            migrationBuilder.CreateSequence<int>(
                name: "AssetNumberSequence",
                startValue: 100000L,
                maxValue: 999999L);

            migrationBuilder.AlterColumn<string>(
                name: "Hostname",
                table: "Instances",
                maxLength: 63,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AddColumn<int>(
                name: "AssetNumber",
                table: "Instances",
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR AssetNumberSequence");

            migrationBuilder.CreateIndex(
                name: "IX_Instances_AssetNumber",
                table: "Instances",
                column: "AssetNumber",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Instances_AssetNumber",
                table: "Instances");

            migrationBuilder.DropSequence(
                name: "AssetNumberSequence");

            migrationBuilder.DropColumn(
                name: "AssetNumber",
                table: "Instances");

            migrationBuilder.AlterColumn<string>(
                name: "Hostname",
                table: "Instances",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 63,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Instances_Hostname",
                table: "Instances",
                column: "Hostname",
                unique: true);
        }
    }
}
