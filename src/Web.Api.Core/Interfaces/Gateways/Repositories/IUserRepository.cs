using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Dto.GatewayResponses;

namespace Web.Api.Core.Interfaces.Gateways.Repositories
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        Task<User> GetUserByIdAsync(string id);
        Task<IEnumerable<User>> GetAllUsersAsync(int pageSize);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<GatewayResponseBase> CreateUser(User user);
        Task<GatewayResponseBase> UpdateUser(User user);
        Task<GatewayResponseBase> DeleteUser(User user);
    }

    public interface IModelRepository : IRepositoryBase<Model>
    {
        Task<Model> GetModelByIdAsync(string id);
    }
}
