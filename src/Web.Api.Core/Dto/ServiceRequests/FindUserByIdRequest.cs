namespace Web.Api.Core.Dto.ServiceRequests
{
    public class FindUserByIdRequest
    {
        public string UserId { get; }

        public FindUserByIdRequest(string userId) => UserId = userId;
    }
}