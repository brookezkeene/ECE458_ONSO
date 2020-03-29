using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Api.Infrastructure.Migrations.ApplicationDb
{
    public partial class ModifyChangePlannerFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExecutedData",
                table: "ChangePlans");

            migrationBuilder.AddColumn<Guid>(
                name: "DatacenterId",
                table: "ChangePlans",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "ExecutedDate",
                table: "ChangePlans",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ChangePlans",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExecutionType",
                table: "ChangePlanItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DatacenterId",
                table: "ChangePlans");

            migrationBuilder.DropColumn(
                name: "ExecutedDate",
                table: "ChangePlans");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ChangePlans");

            migrationBuilder.DropColumn(
                name: "ExecutionType",
                table: "ChangePlanItems");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExecutedData",
                table: "ChangePlans",
                type: "datetime2",
                nullable: true);
        }
    }
}
