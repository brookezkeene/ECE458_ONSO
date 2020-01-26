using System.Collections.Generic;
using System.Linq;
using Moq;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Dto.Errors;
using Web.Api.Core.Dto.ServiceRequests;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Services;
using Xunit;

namespace Web.Api.Core.UnitTests.Services.UserQuery
{
    public class QueryAllTests
    {
        private static User[] BuildFullUserList()
        {
            var user = new User("username", "email", "Display Name", "id", "passwordHash");
            // reuse the same user repeatedly to save having to call this long constructor several times
            return new[] {
                user, user, user, user, user,
                user, user, user, user, user,
                user, user, user, user, user,
                user, user, user, user, user
            };
        }
        [Fact]
        public async void QueryAll_NoPageSizeGiven_ReturnsAllUsers()
        {
            var allUsers = BuildFullUserList();

            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository
                .Setup(repo => repo.GetAllUsersAsync())
                .ReturnsAsync(allUsers);

            var service = new UserQueryService(mockUserRepository.Object);

            var response = await service.Handle(new QueryAllUsersRequest());

            Assert.Equal(allUsers.Length, response.Users.Count());
        }

        [Fact]
        public async void QueryAll_NonPositivePageSize_ReturnsError()
        {
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository
                .Setup(repo => repo.GetAllUsersAsync(It.IsInRange(int.MinValue, 0, Range.Inclusive)));

            var service = new UserQueryService(mockUserRepository.Object);

            var response1 = await service.Handle(new QueryAllUsersRequest(0));
            var response2 = await service.Handle(new QueryAllUsersRequest(-100));

            Assert.False(response1.Succeeded);
            Assert.False(response2.Succeeded);
            Assert.Contains(response1.Errors, error => error is InvalidPageSize);
            Assert.Contains(response2.Errors, error => error is InvalidPageSize);
        }

        [Fact]
        public async void QueryAll_PositivePageSize_RespectedByResponse()
        {
            const int pageSize = 10;
            var limitedUsers = BuildFullUserList()
                .Take(pageSize);
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository
                .Setup(repo => repo.GetAllUsersAsync(It.IsInRange(1, int.MaxValue, Range.Inclusive)))
                .ReturnsAsync(limitedUsers);

            var service = new UserQueryService(mockUserRepository.Object);

            var response = await service.Handle(new QueryAllUsersRequest(pageSize));

            Assert.True(response.Succeeded);
            Assert.Equal(pageSize, response.Users.Count());
        }
    }
}
