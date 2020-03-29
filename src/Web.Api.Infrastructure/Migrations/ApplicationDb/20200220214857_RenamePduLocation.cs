using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Api.Infrastructure.Migrations.ApplicationDb
{
    public partial class RenamePduLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocationEnum",
                table: "Pdu");

            migrationBuilder.AddColumn<int>(
                name: "Location",
                table: "Pdu",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "Pdu");

            migrationBuilder.AddColumn<int>(
                name: "LocationEnum",
                table: "Pdu",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
