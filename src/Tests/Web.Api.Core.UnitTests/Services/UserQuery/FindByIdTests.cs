using Moq;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Dto.Errors;
using Web.Api.Core.Dto.ServiceRequests;
using Web.Api.Core.Interfaces.Repositories;
using Web.Api.Core.Services;
using Xunit;

namespace Web.Api.Core.UnitTests.Services.UserQuery
{
    public class FindByIdTests
    {
        [Fact]
        public async void FindById_ValidId_ReturnsFoundUser()
        {
            const string id = "foo-id";
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository
                .Setup(repo => repo.FindById(id))
                .ReturnsAsync(new User(null, null, null, id, null));

            var service = new UserQueryService(mockUserRepository.Object);

            var response = await service.Handle(new FindUserByIdRequest(id));

            Assert.Equal(id, response.User.Id);
        }

        [Fact]
        public async void FindById_ValidId_ReturnsSuccessTrue()
        {
            const string id = "foo-id";
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository
                .Setup(repo => repo.FindById(id))
                .ReturnsAsync(new User(null, null, null, id, null));

            var service = new UserQueryService(mockUserRepository.Object);

            var response = await service.Handle(new FindUserByIdRequest(id));

            Assert.True(response.Success);
        }

        [Fact]
        public async void FindById_NoSuchUser_ReturnsNullUser()
        {
            const string id = "nonexistent";
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository
                .Setup(repo => repo.FindById(id))
                .ReturnsAsync(() => null);

            var service = new UserQueryService(mockUserRepository.Object);

            var response = await service.Handle(new FindUserByIdRequest(id));

            Assert.Null(response.User);
        }

        [Fact]
        public async void FindById_NoSuchUser_ReturnsSuccessFalse()
        {
            const string id = "nonexistent";
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository
                .Setup(repo => repo.FindById(id))
                .ReturnsAsync(() => null);

            var service = new UserQueryService(mockUserRepository.Object);

            var response = await service.Handle(new FindUserByIdRequest(id));

            Assert.False(response.Success);
        }

        [Fact]
        public async void FindById_NoSuchUser_ReturnsUserNotFoundError()
        {
            const string id = "nonexistent";
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository
                .Setup(repo => repo.FindById(id))
                .ReturnsAsync(() => null);

            var service = new UserQueryService(mockUserRepository.Object);

            var response = await service.Handle(new FindUserByIdRequest(id));

            Assert.Contains(response.Errors, error => error is UserNotFound);
        }
    }
}
