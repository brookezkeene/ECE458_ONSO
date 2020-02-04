namespace Web.Api.Resources
{
    public class ApiErrorResources : IApiErrorResources
    {
        public ApiError CannotSetId()
        {
            return new ApiError
            {
                Code = nameof(CannotSetId),
                Description = ApiErrorResource.CannotSetId
            };
        }
    }
}