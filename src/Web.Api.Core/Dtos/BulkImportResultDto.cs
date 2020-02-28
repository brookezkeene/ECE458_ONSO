namespace Web.Api.Core.Dtos
{
    public class BulkImportResultDto
    {
    
        public BulkImportResultDto(int added, int updated)
        {
            Added = added;
            Updated = updated;
        }

        public int Added { get; set; }
        public int Updated { get; set; }
    }
}