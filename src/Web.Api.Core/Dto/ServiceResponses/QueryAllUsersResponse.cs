using System.Collections.Generic;
using Web.Api.Core.Domain.Entities;

namespace Web.Api.Core.Dto.ServiceResponses
{
    public class QueryAllUsersResponse : ServiceResponseBase
    {
        public IEnumerable<User> Users { get; set; }

        public QueryAllUsersResponse(IEnumerable<Error> errors) : base(errors) { }
        
        public QueryAllUsersResponse(params Error[] errors) : base(errors) { }

        public QueryAllUsersResponse(IEnumerable<User> users) : base(true) => Users = users;

    }
}