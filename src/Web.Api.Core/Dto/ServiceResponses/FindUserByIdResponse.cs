using System.Collections.Generic;
using Web.Api.Core.Domain.Entities;

namespace Web.Api.Core.Dto.ServiceResponses
{
    public class FindUserByIdResponse : ServiceResponseBase
    {
        public User User { get; }

        public FindUserByIdResponse(IEnumerable<Error> errors) : base(errors) { }

        public FindUserByIdResponse(params Error[] errors) : base(errors) { }

        public FindUserByIdResponse(User user) : base(true) => User = user;

    }
}