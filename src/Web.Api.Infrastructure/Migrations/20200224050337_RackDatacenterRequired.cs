using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Api.Infrastructure.Migrations
{
    public partial class RackDatacenterRequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Racks_Datacenters_DatacenterId",
                table: "Racks");

            migrationBuilder.DropIndex(
                name: "IX_Racks_DatacenterId_Row_Column",
                table: "Racks");

            migrationBuilder.AlterColumn<Guid>(
                name: "DatacenterId",
                table: "Racks",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Racks_DatacenterId_Row_Column",
                table: "Racks",
                columns: new[] { "DatacenterId", "Row", "Column" },
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

            migrationBuilder.DropIndex(
                name: "IX_Racks_DatacenterId_Row_Column",
                table: "Racks");

            migrationBuilder.AlterColumn<Guid>(
                name: "DatacenterId",
                table: "Racks",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.CreateIndex(
                name: "IX_Racks_DatacenterId_Row_Column",
                table: "Racks",
                columns: new[] { "DatacenterId", "Row", "Column" },
                unique: true,
                filter: "[DatacenterId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Racks_Datacenters_DatacenterId",
                table: "Racks",
                column: "DatacenterId",
                principalTable: "Datacenters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
