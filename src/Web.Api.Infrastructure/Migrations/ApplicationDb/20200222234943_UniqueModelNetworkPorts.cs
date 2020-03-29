using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Api.Infrastructure.Migrations.ApplicationDb
{
    public partial class UniqueModelNetworkPorts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ModelNetworkPort_ModelId",
                table: "ModelNetworkPort");

            migrationBuilder.CreateIndex(
                name: "IX_ModelNetworkPort_ModelId_Name",
                table: "ModelNetworkPort",
                columns: new[] { "ModelId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ModelNetworkPort_ModelId_Number",
                table: "ModelNetworkPort",
                columns: new[] { "ModelId", "Number" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ModelNetworkPort_ModelId_Name",
                table: "ModelNetworkPort");

            migrationBuilder.DropIndex(
                name: "IX_ModelNetworkPort_ModelId_Number",
                table: "ModelNetworkPort");

            migrationBuilder.CreateIndex(
                name: "IX_ModelNetworkPort_ModelId",
                table: "ModelNetworkPort",
                column: "ModelId");
        }
    }
}
