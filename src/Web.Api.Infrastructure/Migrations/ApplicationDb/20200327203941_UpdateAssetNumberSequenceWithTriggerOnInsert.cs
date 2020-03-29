using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Api.Infrastructure.Migrations.ApplicationDb
{
    public partial class UpdateAssetNumberSequenceWithTriggerOnInsert : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE TRIGGER [update_asset_number_sequence]
                                    ON [Assets]
                                    FOR INSERT
                                    AS
                                    BEGIN
	                                    DECLARE @max_asset_number INT;
	                                    DECLARE @s NVARCHAR(100);
	                                    SET @max_asset_number = (SELECT MAX([AssetNumber]) FROM [Assets]);
	                                    SET @s = N'
		                                    ALTER SEQUENCE
			                                    [AssetNumberSequence]
		                                    RESTART WITH ' + CAST(@max_asset_number + 1 AS NVARCHAR(6));
	                                    EXEC (@s);
                                    END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP TRIGGER [update_asset_number_sequence]");
        }
    }
}
