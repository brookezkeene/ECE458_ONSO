using System;
using System.Security.Authentication;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Web.Api.Common;
using Web.Api.Core.Auth.Interfaces;
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
        Task<Token> LoginAsync(LoginDto login);
    }

    public class IdentityService : IIdentityService
    {
        private readonly IIdentityRepository _identityRepository;
        private readonly IJwtFactory _jwtFactory;

        public IdentityService(IIdentityRepository identityRepository, IJwtFactory jwtFactory)
        {
            _identityRepository = identityRepository;
            _jwtFactory = jwtFactory;
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

        public async Task<Token> LoginAsync(LoginDto login)
        {
            if (!string.IsNullOrEmpty(login.Username) && !string.IsNullOrEmpty(login.Password))
            {
                // confirm we have a user with the given username
                var user = await _identityRepository.FindByNameAsync(login.Username);
                if (user != null)
                {
                    // validate password
                    if (await _identityRepository.CheckPassword(user, login.Password))
                    {
                        return await _jwtFactory.GenerateEncodedToken(user.Id, user.UserName);
                    }
                }
            }
            throw new InvalidCredentialException("Invalid username or password.");
        }
    }
}