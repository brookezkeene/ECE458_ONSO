namespace Web.Api.Core.Dto
{
    public class Error
    {
        public string Description { get; }

        public Error(string description) => Description = description;
    }
}