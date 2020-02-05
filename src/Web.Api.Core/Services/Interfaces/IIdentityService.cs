using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Web.Api.Common;
using Web.Api.Core.Dtos;

namespace Web.Api.Core.Services.Interfaces
{
    public interface IIdentityService
    {
        Task<PagedList<FlatUserDto>> GetUsersAsync(string search, int page = 1, int pageSize = 10);
        Task<FlatUserDto> GetUserAsync(Guid userId);
        Task<(IdentityResult identityResult, Guid userId)> CreateUserAsync(RegisterUserDto user);
        Task<Token> LoginAsync(LoginDto login);
    }
}