using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Dto;

namespace Web.Api.Core.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User> FindById(string id);
        Task<IEnumerable<User>> QueryAll(int? pageSize);
        Task<IEnumerable<User>> QueryAll();
    }
}
