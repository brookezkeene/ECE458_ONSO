using System.Collections.Generic;
using Web.Api.Core.Domain.Entities;

namespace Web.Api.Core.Dto.ServiceResponses
{
    public class QueryAllUsersResponse : BaseResponse
    {
        public IEnumerable<User> Users { get; set; }

        public QueryAllUsersResponse(IEnumerable<Error> errors) : base(false, errors) { }
        
        public QueryAllUsersResponse(Error error) : base(false, new[] { error }) { }

        public QueryAllUsersResponse(IEnumerable<User> users) : base(true, null) => Users = users;

    }
}