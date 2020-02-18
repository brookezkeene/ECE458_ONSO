using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Api.Infrastructure.Migrations
{
    public partial class AddDatacenters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DatacenterId",
                table: "Racks",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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

            migrationBuilder.CreateIndex(
                name: "IX_Racks_DatacenterId",
                table: "Racks",
                column: "DatacenterId");

            migrationBuilder.CreateIndex(
                name: "IX_Datacenters_Name",
                table: "Datacenters",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Racks_Datacenters_DatacenterId",
                table: "Racks",
                column: "DatacenterId",
                principalTable: "Datacenters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Racks_Datacenters_DatacenterId",
                table: "Racks");

            migrationBuilder.DropTable(
                name: "Datacenters");

            migrationBuilder.DropIndex(
                name: "IX_Racks_DatacenterId",
                table: "Racks");

            migrationBuilder.DropColumn(
                name: "DatacenterId",
                table: "Racks");
        }
    }
}
