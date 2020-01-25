using System.Collections.Generic;
using Web.Api.Core.Domain.Entities;

namespace Web.Api.Core.Dto.ServiceResponses
{
    public class FindUserByIdResponse : BaseResponse
    {
        public User User { get; }

        public FindUserByIdResponse(IEnumerable<Error> errors) : base(false, errors) { }

        public FindUserByIdResponse(Error error) : base(false, new[] { error }) { }

        public FindUserByIdResponse(User user) : base(true, null) => User = user;

    }
}