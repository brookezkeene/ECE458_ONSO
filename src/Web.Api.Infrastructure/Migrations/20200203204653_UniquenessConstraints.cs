using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Api.Infrastructure.Migrations
{
    public partial class UniquenessConstraints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Row",
                table: "Racks",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Racks_Row_Column",
                table: "Racks",
                columns: new[] { "Row", "Column" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Models_Vendor_ModelNumber",
                table: "Models",
                columns: new[] { "Vendor", "ModelNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Instances_Hostname",
                table: "Instances",
                column: "Hostname",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Racks_Row_Column",
                table: "Racks");

            migrationBuilder.DropIndex(
                name: "IX_Models_Vendor_ModelNumber",
                table: "Models");

            migrationBuilder.DropIndex(
                name: "IX_Instances_Hostname",
                table: "Instances");

            migrationBuilder.AlterColumn<string>(
                name: "Row",
                table: "Racks",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
