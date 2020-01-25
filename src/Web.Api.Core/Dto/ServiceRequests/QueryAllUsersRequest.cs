namespace Web.Api.Core.Dto.ServiceRequests
{
    public class QueryAllUsersRequest
    {
        public int? PageSize;

        public QueryAllUsersRequest(int? pageSize)
        {
            PageSize = pageSize;
        }

        public QueryAllUsersRequest() : this(null) { }
    }
}