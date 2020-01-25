using System.Threading.Tasks;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Dto.Errors;
using Web.Api.Core.Dto.ServiceRequests;
using Web.Api.Core.Dto.ServiceResponses;
using Web.Api.Core.Interfaces;
using Web.Api.Core.Interfaces.Repositories;

namespace Web.Api.Core.Services
{
    public class UserQueryService : IRequestHandler<FindUserByIdRequest, FindUserByIdResponse>,
                                    IRequestHandler<QueryAllUsersRequest, QueryAllUsersResponse>
    {
        private readonly IUserRepository _userRepository;

        public UserQueryService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<FindUserByIdResponse> Handle(FindUserByIdRequest request)
        {
            var user = await _userRepository.FindById(request.UserId);
            return user != null ? new FindUserByIdResponse(user) : new FindUserByIdResponse(new UserNotFound());
        }

        public async Task<QueryAllUsersResponse> Handle(QueryAllUsersRequest request)
        {
            if (request.PageSize <= 0) return new QueryAllUsersResponse(new InvalidPageSize());

            var users = request.PageSize.HasValue
                ? await _userRepository.QueryAll(request.PageSize)
                : await _userRepository.QueryAll();
            return new QueryAllUsersResponse(users);

        }
    }
}
