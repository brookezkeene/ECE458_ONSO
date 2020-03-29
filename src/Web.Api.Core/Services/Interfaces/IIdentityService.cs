using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Web.Api.Common;
using Web.Api.Core.Dtos;

namespace Web.Api.Core.Services.Interfaces
{
    public interface IIdentityService
    {
        Task<PagedList<UserDto>> GetUsersAsync(string search, int page = 1, int pageSize = 10);
        Task<UserDto> GetUserAsync(Guid userId);
        Task<(IdentityResult identityResult, Guid userId)> CreateUserAsync(RegisterUserDto user);
        Task<IdentityResult> DeleteUserAsync(Guid userId);
    }
}