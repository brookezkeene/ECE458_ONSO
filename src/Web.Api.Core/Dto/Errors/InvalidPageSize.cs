namespace Web.Api.Core.Dto.Errors
{
    public class InvalidPageSize : Error
    {
        public InvalidPageSize(string description = null) : base(description) { }
    }
}