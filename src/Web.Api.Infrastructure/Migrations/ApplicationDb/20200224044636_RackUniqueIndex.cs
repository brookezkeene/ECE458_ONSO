using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Api.Infrastructure.Migrations.ApplicationDb
{
    public partial class RackUniqueIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Racks_DatacenterId",
                table: "Racks");

            migrationBuilder.DropIndex(
                name: "IX_Racks_Row_Column",
                table: "Racks");

            migrationBuilder.CreateIndex(
                name: "IX_Racks_DatacenterId_Row_Column",
                table: "Racks",
                columns: new[] { "DatacenterId", "Row", "Column" },
                unique: true,
                filter: "[DatacenterId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Racks_DatacenterId_Row_Column",
                table: "Racks");

            migrationBuilder.CreateIndex(
                name: "IX_Racks_DatacenterId",
                table: "Racks",
                column: "DatacenterId");

            migrationBuilder.CreateIndex(
                name: "IX_Racks_Row_Column",
                table: "Racks",
                columns: new[] { "Row", "Column" },
                unique: true);
        }
    }
}
