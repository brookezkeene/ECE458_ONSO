using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Web.Api.Common;
using Web.Api.Common.Extensions;
using Web.Api.Infrastructure.Entities;

namespace Web.Api.Infrastructure.Repositories.Interfaces
{
    public interface IIdentityRepository
    {
        Task<PagedList<User>> GetUsersAsync(string search, int page = 1, int pageSize = 10);
        Task<User> GetUserAsync(Guid userId);
        Task<(IdentityResult identityResult, Guid userId)> CreateUserAsync(User user, string password);
        Task<User> FindByNameAsync(string username);
        Task<bool> CheckPassword(User user, string password);
    }

    public class IdentityRepository : IIdentityRepository
    {
        private readonly UserManager<User> _userManager;

        public IdentityRepository(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<PagedList<User>> GetUsersAsync(string search, int page = 1, int pageSize = 10)
        {
            var pagedList = new PagedList<User>();
            Expression<Func<User, bool>> searchCondition = x =>
                x.UserName.Contains(search) || x.Email.Contains(search) || x.FirstName.Contains(search) ||
                x.LastName.Contains(search);

            var users = await _userManager.Users.WhereIf(!string.IsNullOrEmpty(search), searchCondition)
                .PageBy(x => x.Id, page, pageSize)
                .ToListAsync();

            pagedList.AddRange(users);
            pagedList.TotalCount = await _userManager.Users.WhereIf(!string.IsNullOrEmpty(search), searchCondition)
                .CountAsync();
            pagedList.PageSize = pageSize;
            pagedList.CurrentPage = page;

            return pagedList;
        }

        public async Task<User> GetUserAsync(Guid userId)
        {
            return await _userManager.FindByIdAsync(userId.ToString());
        }

        public async Task<(IdentityResult identityResult, Guid userId)> CreateUserAsync(User user, string password)
        {
            var identityResult = await _userManager.CreateAsync(user, password);

            return (identityResult, Guid.Parse(user.Id));
        }

        public async Task<User> FindByNameAsync(string username)
        {
            return await _userManager.FindByNameAsync(username);

        }

        public async Task<bool> CheckPassword(User user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }
    }
}
