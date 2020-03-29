using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Web.Api.Common;
using Web.Api.Infrastructure.Entities;

namespace Web.Api.Infrastructure.Repositories.Interfaces
{
    public interface IIdentityRepository
    {
        Task<PagedList<User>> GetUsersAsync(string search, int page = 1, int pageSize = 10);
        Task<User> GetUserAsync(Guid userId);
        Task<(IdentityResult identityResult, Guid userId)> CreateUserAsync(User user, string password);
        Task<IdentityResult> DeleteUserAsync(User user);
        Task<User> FindByNameAsync(string username);
        Task<bool> CheckPassword(User user, string password);

        User GetUser(string username);
    }
}
