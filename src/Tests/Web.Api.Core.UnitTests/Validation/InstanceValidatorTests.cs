using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentValidation.TestHelper;
using Moq;
using Web.Api.Core.Validation;
using Web.Api.Infrastructure.Entities;
using Web.Api.Infrastructure.Repositories.Interfaces;
using Xunit;

namespace Web.Api.Core.UnitTests.Validation
{
    public class InstanceValidatorTests
    {
        [Fact]
        public async void InstanceValidator_FailsIfInstanceDoesNotFitInRack()
        {
            var instance = GetInstanceThatDoesNotFitInRack();
            var sut = GetDefaultValidator();

            var result = await sut.ValidateAsync(instance);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.ErrorCode == "DoesNotFit");
        }

        [Fact]
        public async void InstanceValidator_FailsIfInstanceHasConflict()
        {
            var instance = GetInstanceWithConflict();
            var sut = GetDefaultValidator();

            var result = await sut.ValidateAsync(instance);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.ErrorCode == "Conflict");
        }

        [Fact]
        public async void InstanceValidator_PassesIfValidInstance()
        {
            var instance = GetValidInstance();
            var sut = GetAllGreenValidator();

            var result = await sut.ValidateAsync(instance);

            Assert.True(result.IsValid);
        }

        private static InstanceValidator GetAllGreenValidator()
        {
            var mockInstanceRepo = new Mock<IInstanceRepository>();
            mockInstanceRepo.Setup(o => o.InstanceIsUniqueAsync(It.IsAny<string>(), It.IsAny<Guid>()))
                .ReturnsAsync(true);

            var mockRackRepo = new Mock<IRackRepository>();
            mockRackRepo.Setup(o => o.AddressExistsAsync(It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(true);

            var mockModelRepo = new Mock<IModelRepository>();
            mockModelRepo.Setup(o => o.ModelExistsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Guid>()))
                .ReturnsAsync(true);

            var mockIdentityRepo = new Mock<IIdentityRepository>();
            mockIdentityRepo.Setup(o => o.GetUserAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new User()); // not null

            return new InstanceValidator(mockInstanceRepo.Object, mockRackRepo.Object, mockModelRepo.Object,
                mockIdentityRepo.Object);
        }

        private static InstanceValidator GetDefaultValidator()
        {
            var mockInstanceRepo = new Mock<IInstanceRepository>();
            var mockRackRepo = new Mock<IRackRepository>();
            var mockModelRepo = new Mock<IModelRepository>();
            var mockIdentityRepo = new Mock<IIdentityRepository>();

            return new InstanceValidator(mockInstanceRepo.Object, mockRackRepo.Object, mockModelRepo.Object,
                mockIdentityRepo.Object);
        }

        private static Instance GetInstanceThatDoesNotFitInRack()
        {
            var rack = new Rack();
            var model = new Model {Height = 4};
            var instance = new Instance() {RackPosition = 40, Rack = rack, Model = model};

            rack.Instances = new List<Instance> {instance};
            model.Instances = new List<Instance> {instance};

            return instance;
        }

        private static Instance GetInstanceWithConflict()
        {
            var rack = new Rack();
            var model = new Model { Height = 4 };
            var instance = new Instance() { RackPosition = 5, Rack = rack, Model = model };
            var conflict = new Instance() { RackPosition = 3, Rack = rack, Model = model };

            rack.Instances = new List<Instance> { instance, conflict };
            model.Instances = new List<Instance> { instance, conflict };

            return instance;
        }

        private static Instance GetValidInstance()
        {
            var rack = new Rack();
            var model = new Model() {Height = 4};
            var instance = new Instance() {RackPosition = 5, Hostname = "server9", Rack = rack, Model = model};
            var otherNoConflict = new Instance() {RackPosition = 9, Hostname = "server8", Rack = rack, Model = model};

            rack.Instances = new List<Instance> {instance, otherNoConflict};
            model.Instances = new List<Instance> { instance, otherNoConflict };

            return instance;
        }
    }
}
