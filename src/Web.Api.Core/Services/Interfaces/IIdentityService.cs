using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Web.Api.Common;
using Web.Api.Core.Dtos;
using Web.Api.Core.Mappers;
using Web.Api.Infrastructure.Repositories.Interfaces;

namespace Web.Api.Core.Services.Interfaces
{
    public interface IIdentityService
    {
        Task<PagedList<FlatUserDto>> GetUsersAsync(string search, int page = 1, int pageSize = 10);
        Task<FlatUserDto> GetUserAsync(Guid userId);
        Task<(IdentityResult identityResult, Guid userId)> CreateUserAsync(RegisterUserDto user);
    }

    public class IdentityService : IIdentityService
    {
        private readonly IIdentityRepository _identityRepository;

        public IdentityService(IIdentityRepository identityRepository)
        {
            _identityRepository = identityRepository;
        }

        public async Task<PagedList<FlatUserDto>> GetUsersAsync(string search, int page = 1, int pageSize = 10)
        {
            var pagedList = await _identityRepository.GetUsersAsync(search, page, pageSize);
            return pagedList.ToDto();
        }

        public async Task<FlatUserDto> GetUserAsync(Guid userId)
        {
            var user = await _identityRepository.GetUserAsync(userId);
            return user.ToDto();
        }

        public async Task<(IdentityResult identityResult, Guid userId)> CreateUserAsync(RegisterUserDto user)
        {
            var userIdentity = user.ToEntity();
            var (identityResult, userId) = await _identityRepository.CreateUserAsync(userIdentity, user.Password);

            return (identityResult, userId);
        }
    }
}