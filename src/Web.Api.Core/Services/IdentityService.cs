using System;
using System.Security.Authentication;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Web.Api.Common;
using Web.Api.Core.Auth.Interfaces;
using Web.Api.Core.Dtos;
using Web.Api.Core.Mappers;
using Web.Api.Core.Services.Interfaces;
using Web.Api.Infrastructure.Entities;
using Web.Api.Infrastructure.Repositories.Interfaces;

namespace Web.Api.Core.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IIdentityRepository _identityRepository;
        private readonly IMapper _mapper;

        public IdentityService(IIdentityRepository identityRepository, IMapper mapper)
        {
            _identityRepository = identityRepository;
            _mapper = mapper;
        }

        public async Task<PagedList<UserDto>> GetUsersAsync(string search, int page = 1, int pageSize = 10)
        {
            var pagedList = await _identityRepository.GetUsersAsync(search, page, pageSize);
            return _mapper.Map<PagedList<UserDto>>(pagedList);
        }

        public async Task<UserDto> GetUserAsync(Guid userId)
        {
            var user = await _identityRepository.GetUserAsync(userId);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<(IdentityResult identityResult, Guid userId)> CreateUserAsync(RegisterUserDto user)
        {
            var userIdentity = _mapper.Map<User>(user);
            var (identityResult, userId) = await _identityRepository.CreateUserAsync(userIdentity, user.Password);

            return (identityResult, userId);
        }

        public async Task<IdentityResult> DeleteUserAsync(Guid userId)
        {
            var user = await _identityRepository.GetUserAsync(userId);
            var identityResult = await _identityRepository.DeleteUserAsync(user);

            return identityResult;
        }
    }
}