namespace Web.Api.Core.Dto
{
    public class Error
    {
        public string Code { get; }
        public string Description { get; }

        public Error(string code, string description)
            => (Code, Description) = (code, description);

        public static Error Default => new Error("500", "An unknown error has occurred.");
    }
}