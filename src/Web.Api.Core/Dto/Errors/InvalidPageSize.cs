namespace Web.Api.Core.Dto.Errors
{
    public class InvalidPageSize : Error
    {
        public InvalidPageSize(string description = "Invalid value for page size parameter") : base("400", description) { }
    }
}